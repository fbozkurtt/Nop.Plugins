using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Services.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public class CustomSpecificationAttributeParser : ICustomSpecificationAttributeParser
    {
        #region Fields

        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;
        private readonly ISpecificationAttributeService _specificationAttributeService;

        #endregion

        #region Ctor

        public CustomSpecificationAttributeParser(ICustomSpecificationAttributeService customSpecificationAttributeService, ISpecificationAttributeService specificationAttributeService)
        {
            _customSpecificationAttributeService = customSpecificationAttributeService;
            _specificationAttributeService = specificationAttributeService;
        }

        #endregion

        #region Utilities

        protected IList<int> ParseCustomSpecificationAttributeIds(string attributesXml)
        {
            var ids = new List<int>();
            if (string.IsNullOrEmpty(attributesXml))
                return ids;

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(attributesXml);

                foreach (XmlNode node in xmlDoc.SelectNodes(@"//Attributes/CustomSpecificationAttribute"))
                {
                    if (node.Attributes?["ID"] == null)
                        continue;

                    var str1 = node.Attributes["ID"].InnerText.Trim();
                    if (int.TryParse(str1, out var id))
                        ids.Add(id);
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }

            return ids;
        }

        #endregion

        #region Methods

        public string AddCustomSpecificationAttribute(string attributesXml, CustomSpecificationAttribute customSpecificationAttribute, IList<string> options)
        {
            var result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(attributesXml))
                {
                    var element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(element1);
                }
                else
                    xmlDoc.LoadXml(attributesXml);

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement attributeElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CustomSpecificationAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["ID"] == null)
                        continue;

                    var str1 = node1.Attributes["ID"].InnerText.Trim();

                    if (!int.TryParse(str1, out var id))
                        continue;

                    if (id != customSpecificationAttribute.SpecificationAttributeId)
                        continue;

                    attributeElement = (XmlElement)node1;
                    break;
                }

                //create new one if not found
                if (attributeElement == null)
                {
                    attributeElement = xmlDoc.CreateElement("CustomSpecificationAttribute");
                    attributeElement.SetAttribute("ID", customSpecificationAttribute.Id.ToString());
                    rootElement.AppendChild(attributeElement);
                }

                var attributeValueElement = xmlDoc.CreateElement("SpecificationAttributeOption");
                attributeElement.AppendChild(attributeValueElement);

                var attributeOptionValueElement = xmlDoc.CreateElement("Option");
                attributeOptionValueElement.InnerText = option;
                attributeValueElement.AppendChild(attributeOptionValueElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }

            return result;
        }

        public async Task<bool?> IsConditionMetAsync(CustomSpecificationAttribute customSpecificationAttribute, string selectedAttributesXml)
        {
            if (customSpecificationAttribute == null)
                throw new ArgumentNullException(nameof(customSpecificationAttribute));

            var conditionAttributeXml = customSpecificationAttribute.ConditionAttributeXml;
            if (string.IsNullOrEmpty(conditionAttributeXml))
                //no condition
                return null;

            //load an attribute this one depends on
            var dependOnAttribute = (await ParseCustomSpecificationAttributesAsync(conditionAttributeXml)).FirstOrDefault();
            if (dependOnAttribute == null)
                return true;

            var optionsThatShouldBeSelected = ParseOptions(conditionAttributeXml, dependOnAttribute.Id)
                //a workaround here:
                //ConditionAttributeXml can contain "empty" values (nothing is selected)
                //but in other cases (like below) we do not store empty values
                //that's why we remove empty values here
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();
            var selectedValues = ParseOptions(selectedAttributesXml, dependOnAttribute.Id);
            if (optionsThatShouldBeSelected.Count != selectedValues.Count)
                return false;

            //compare values
            var allFound = true;
            foreach (var t1 in optionsThatShouldBeSelected)
            {
                var found = false;
                foreach (var t2 in selectedValues)
                    if (t1 == t2)
                        found = true;
                if (!found)
                    allFound = false;
            }

            return allFound;
        }

        public async Task<IList<CustomSpecificationAttribute>> ParseCustomSpecificationAttributesAsync(string attributesXml)
        {
            var result = new List<CustomSpecificationAttribute>();
            if (string.IsNullOrEmpty(attributesXml))
                return result;

            var ids = ParseCustomSpecificationAttributeIds(attributesXml);
            foreach (var id in ids)
            {
                var attribute = await _customSpecificationAttributeService.GetCustomSpecificationAttributeByIdAsync(id);
                if (attribute != null)
                    result.Add(attribute);
            }

            return result;
        }

        public async IAsyncEnumerable<(CustomSpecificationAttribute attribute, IAsyncEnumerable<SpecificationAttributeOption> options)> ParseSpecificationAttributeOptions(string attributesXml)
        {
            if (string.IsNullOrEmpty(attributesXml))
                yield break;

            var attributes = await ParseCustomSpecificationAttributesAsync(attributesXml);

            foreach (var attribute in attributes)
            {
                if (!attribute.ShouldHaveValues())
                    continue;

                var valuesStr = ParseOptions(attributesXml, attribute.Id);

                yield return (attribute, getOptions(valuesStr));
            }

            async IAsyncEnumerable<SpecificationAttributeOption> getOptions(IList<string> valuesStr)
            {
                foreach (var valueStr in valuesStr)
                {
                    if (string.IsNullOrEmpty(valueStr))
                        continue;

                    if (!int.TryParse(valueStr, out var id))
                        continue;

                    var option = await _specificationAttributeService.GetSpecificationAttributeOptionByIdAsync(id);
                    if (option != null)
                        yield return option;
                }
            }
        }

        public IList<string> ParseOptions(string attributesXml, int customSpecificationAttributeId)
        {
            var selectedSpecificationAttributeOptions = new List<string>();
            if (string.IsNullOrEmpty(attributesXml))
                return selectedSpecificationAttributeOptions;

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(attributesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CustomSpecificationAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["ID"] == null)
                        continue;

                    var str1 = node1.Attributes["ID"].InnerText.Trim();
                    if (!int.TryParse(str1, out var id))
                        continue;

                    if (id != customSpecificationAttributeId)
                        continue;

                    var nodeList2 = node1.SelectNodes(@"SpecificationAttributeOption/Option");
                    foreach (XmlNode node2 in nodeList2)
                    {
                        var option = node2.InnerText.Trim();
                        selectedSpecificationAttributeOptions.Add(option);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }

            return selectedSpecificationAttributeOptions;
        }

        public string RemoveCustomSpecificationAttribute(string attributesXml, CustomSpecificationAttribute customSpecificationAttribute)
        {
            var result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(attributesXml))
                {
                    var element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(element1);
                }
                else
                    xmlDoc.LoadXml(attributesXml);

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement attributeElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CustomSpecificationAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["ID"] == null)
                        continue;

                    var str1 = node1.Attributes["ID"].InnerText.Trim();

                    if (!int.TryParse(str1, out var id))
                        continue;

                    if (id != customSpecificationAttribute.Id)
                        continue;

                    attributeElement = (XmlElement)node1;
                    break;
                }

                //found
                if (attributeElement != null)
                    rootElement.RemoveChild(attributeElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }

            return result;
        }

        #endregion
    }
}

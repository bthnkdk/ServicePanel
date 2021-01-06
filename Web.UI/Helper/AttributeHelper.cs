using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.UI
{
    public class AttributeHelper
    {
    }
    public class SwitcheryAttribute : UIHintAttribute, IMetadataAware
    {

        public string SwitcheryFalseText { get; set; }
        public string SwitcheryTrueText { get; set; }


        public SwitcheryAttribute(string uiHint = "BooleanSwitch") : base(uiHint)
        {
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["SwitcheryFalseText"] = SwitcheryFalseText;
            metadata.AdditionalValues["SwitcheryTrueText"] = SwitcheryTrueText;
        }
    }
}
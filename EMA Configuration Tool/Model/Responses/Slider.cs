using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Slider : StringChoice
    {
        public override string ResponseXMLType
        { 
            get 
            {
                return "Slider"; 
            } 
        }

        public Slider()
            : base()
        {
            
        }

        public override ResponseBase Copy()
        {
            Slider slide = new Slider();
            slide.Responses = Responses;
            return slide;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Features.Match3.Scripts.Domain;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public class StepVisualConverterRegistry
    {
        private readonly List<IStepVisualConverter> _converters;

        public StepVisualConverterRegistry()
        {
            _converters = new List<IStepVisualConverter>
            {
                new MatchStepVisualConverter(),
                new GravityStepVisualConverter(),
                new RefillStepVisualConverter()
            };
        }

        public IStepVisualConverter GetConverter(GameStepEntity step)
        {
            var converter = _converters.FirstOrDefault(c => c.CanConvert(step));
            if (converter == null)
            {
                throw new InvalidOperationException($"No converter found for step type: {step.GetType().Name}");
            }
            return converter;
        }
    }
}

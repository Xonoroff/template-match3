using System.Collections.Generic;
using Features.Match3.Scripts.Domain;
using Features.Match3.Scripts.Views;
using UnityEngine;

namespace Features.Match3.Scripts.Presenters.StepConverters
{
    public interface IStepVisualConverter
    {
        bool CanConvert(GameStepEntity step);
        VisualStep Convert(GameStepEntity step);
    }
}

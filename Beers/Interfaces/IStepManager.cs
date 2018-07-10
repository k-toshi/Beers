using System;
using Beers.Models;
using System.Collections.Generic;
namespace Beers.Interfaces
{
    public interface IStepManager
    {
        void StartStepCounts();
        void StopStepCounts();
        int GetStepCounts();
        int GetStepPoints();
        bool IsCountTargetArea();
        Location GetCurrentLocation();
        bool IsStarted();
    }
}

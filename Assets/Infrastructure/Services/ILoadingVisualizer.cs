using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services;

namespace Assets.Infrastructure.Services.Loading
{
    public interface ILoadingVisualizer : IService
    {
        void ShowLoadbar(float progress);
        void HideLoadbar();
    }
}

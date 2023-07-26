using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Assets;

namespace Assets.Infrastructure.Services.Loading
{
    public class LoadingVisualizer : ILoadingVisualizer
    {
        private IAssetLoader _assetLoader;
        private LoadingScreenUI _loadingScreenUI;
        public LoadingVisualizer(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void CleanUp()
        {

        }

        public void HideLoadbar()
        {
            if (_loadingScreenUI != null)
                _loadingScreenUI.Hide();
        }

        public void ShowLoadbar(float progress)
        {
            if (_loadingScreenUI == null)
                _loadingScreenUI = _assetLoader.Instantiate<LoadingScreenUI>(AssetPaths.LoadingScreen);

            _loadingScreenUI.LoadProcess(progress);
        }
    }
}

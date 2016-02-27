using System.Linq;
using System.Web.Mvc;

namespace Alloy
{
    public class FeaturesLayoutViewEngine : RazorViewEngine
    {
        private const string FeaturePlaceholder = "%FEATURE%";

        public FeaturesLayoutViewEngine()
        {
            var viewEnginePaths = new[] {
                "~/Features/" + FeaturePlaceholder + "/{0}.cshtml",
                "~/Features/" + FeaturePlaceholder + "/{0}.vbhtml",
                "~/Features/Shared/{0}.cshtml"
            };
            
            base.PartialViewLocationFormats = viewEnginePaths;
            base.ViewLocationFormats = viewEnginePaths;
            base.MasterLocationFormats = viewEnginePaths;
        }

        /// <summary>
        /// Replaces the %FEATURE% placeholder in the virtualpath and checks if the requested view exists.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="viewPath"></param>
        /// <param name="masterPath"></param>
        /// <returns></returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            if (!viewPath.Contains(FeaturePlaceholder) || controllerContext.Controller == null)
            {
                return base.CreateView(controllerContext, viewPath, masterPath);
            }

            var fullFeaturePath = GetFeaturePath(controllerContext.Controller);

            if (fullFeaturePath == null) return base.CreateView(controllerContext, viewPath, masterPath);

            viewPath = ReplaceFeaturePlaceholder(viewPath, fullFeaturePath);

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        /// <summary>
        /// Replaces the %FEATURE% placeholder in the virtualpath and checks if the requested partial view exists.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="partialPath"></param>
        /// <returns></returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if (!partialPath.Contains(FeaturePlaceholder) || controllerContext.Controller == null)
            {
                return base.CreatePartialView(controllerContext, partialPath);
            }

            var fullFeaturePath = GetFeaturePath(controllerContext.Controller);

            if (fullFeaturePath == null) return base.CreatePartialView(controllerContext, partialPath);

            partialPath = ReplaceFeaturePlaceholder(partialPath, fullFeaturePath);

            return base.CreatePartialView(controllerContext, partialPath);
        }

        /// <summary>
        /// Replaces the %FEATURE% placeholder in the virtualpath and checks if the requested file exists.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            if (!virtualPath.Contains(FeaturePlaceholder) || controllerContext.Controller == null)
            {
                return base.FileExists(controllerContext, virtualPath);
            }

            var fullFeaturePath = GetFeaturePath(controllerContext.Controller);

            if (fullFeaturePath == null) return base.FileExists(controllerContext, virtualPath);

            virtualPath = ReplaceFeaturePlaceholder(virtualPath, fullFeaturePath);

            return base.FileExists(controllerContext, virtualPath);
        }

        /// <summary>
        /// Takes the namespace from the controller
        /// Splits it, removes the first and second item from the namespace(Project name and Features folder.)
        /// Joins together the remaining pieces as a Path to the feature.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private string GetFeaturePath(ControllerBase controller)
        {
            var controllerNamespace = controller.GetType().Namespace;

            if (string.IsNullOrEmpty(controllerNamespace)) return null;

            var sections = controllerNamespace.Split('.').ToList();

            if (sections.Count < 3) return null;

            sections.RemoveAt(0); //Removes the project name.
            sections.RemoveAt(0); //Removes "Features";

            var fullFeaturePath = string.Join("/", sections);

            return fullFeaturePath;
        }

        /// <summary>
        /// Replaces the %FEATURE% placeholder with the actual path.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="featurePath"></param>
        /// <returns></returns>
        private string ReplaceFeaturePlaceholder(string virtualPath, string featurePath)
        {
            return virtualPath.Replace(FeaturePlaceholder, featurePath);
        }
    }
}
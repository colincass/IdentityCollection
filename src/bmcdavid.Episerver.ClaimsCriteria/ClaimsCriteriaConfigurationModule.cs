using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

//namespace bmcdavid.Episerver.ClaimsCriteria
//{
//    /// <summary>
//    /// Constructor
//    /// </summary>
//    [InitializableModule]
//    public class ClaimsCriteriaConfigurationModule : IConfigurableModule
//    {
//        /// <summary>
//        /// Add services
//        /// </summary>
//        /// <param name="context"></param>
//        public void ConfigureContainer(ServiceConfigurationContext context)
//        {
//            context.Services
//                .AddSingleton<IClaimTypeSelectorExtension, DefaultClaimTypeSelectorExtension>()
//                .AddSingleton<IClaimUserTools,DefaultClaimUserTools>()
//                ;

//        }

//        /// <summary>
//        /// Init
//        /// </summary>
//        /// <param name="context"></param>
//        public void Initialize(InitializationEngine context) { }

//        /// <summary>
//        /// Uninit
//        /// </summary>
//        /// <param name="context"></param>
//        public void Uninitialize(InitializationEngine context) { }
//    }
//}
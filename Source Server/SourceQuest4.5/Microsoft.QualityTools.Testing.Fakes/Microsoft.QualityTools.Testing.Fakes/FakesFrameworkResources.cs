using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class FakesFrameworkResources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = resourceMan = new ResourceManager("Microsoft.QualityTools.Testing.Fakes.FakesFrameworkResources", typeof(FakesFrameworkResources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string AMethodFromAMoleWasNotResolvedPleaseRegenerateTheMoles => ResourceManager.GetString("AMethodFromAMoleWasNotResolvedPleaseRegenerateTheMoles", resourceCulture);

		internal static string CannotBeInstrumented => ResourceManager.GetString("CannotBeInstrumented", resourceCulture);

		internal static string CannotInstantiateAbstractType => ResourceManager.GetString("CannotInstantiateAbstractType", resourceCulture);

		internal static string DelegateTargetIsANullReference => ResourceManager.GetString("DelegateTargetIsANullReference", resourceCulture);

		internal static string DetourInstrumentationProviderNotSet => ResourceManager.GetString("DetourInstrumentationProviderNotSet", resourceCulture);

		internal static string DetoursAreNotEnabled => ResourceManager.GetString("DetoursAreNotEnabled", resourceCulture);

		internal static string FailedToGetFunctionAddress => ResourceManager.GetString("FailedToGetFunctionAddress", resourceCulture);

		internal static string FailedToGetProfilerModuleHandle => ResourceManager.GetString("FailedToGetProfilerModuleHandle", resourceCulture);

		internal static string FailedToOpenProfilerRegistryKey => ResourceManager.GetString("FailedToOpenProfilerRegistryKey", resourceCulture);

		internal static string FailedToResolveProfilerPath => ResourceManager.GetString("FailedToResolveProfilerPath", resourceCulture);

		internal static string FailedToSetDetourProvider => ResourceManager.GetString("FailedToSetDetourProvider", resourceCulture);

		internal static string IncompatibleMethodAndDetour => ResourceManager.GetString("IncompatibleMethodAndDetour", resourceCulture);

		internal static string MethodIsAbstract => ResourceManager.GetString("MethodIsAbstract", resourceCulture);

		internal static string MethodIsAStaticConstructor => ResourceManager.GetString("MethodIsAStaticConstructor", resourceCulture);

		internal static string MethodMustBeAFullyInstantiated => ResourceManager.GetString("MethodMustBeAFullyInstantiated", resourceCulture);

		internal static string MustBeAClassOrAValuetype => ResourceManager.GetString("MustBeAClassOrAValuetype", resourceCulture);

		internal static string MustBeAFullyInstantiedMethod => ResourceManager.GetString("MustBeAFullyInstantiedMethod", resourceCulture);

		internal static string MustNotBeAbstract => ResourceManager.GetString("MustNotBeAbstract", resourceCulture);

		internal static string MustNotBeExtern => ResourceManager.GetString("MustNotBeExtern", resourceCulture);

		internal static string MustNotUseTheVarargsCallingConvention => ResourceManager.GetString("MustNotUseTheVarargsCallingConvention", resourceCulture);

		internal static string OptionalReceiverShouldBeNullForStaticMethods => ResourceManager.GetString("OptionalReceiverShouldBeNullForStaticMethods", resourceCulture);

		internal static string ProfilerClassIdTraceMessage => ResourceManager.GetString("ProfilerClassIdTraceMessage", resourceCulture);

		internal static string ProfilerPathTraceMessage => ResourceManager.GetString("ProfilerPathTraceMessage", resourceCulture);

		internal static string ProfilerRegistryPathTraceMessage => ResourceManager.GetString("ProfilerRegistryPathTraceMessage", resourceCulture);

		internal static string ShimsContextNotCreated => ResourceManager.GetString("ShimsContextNotCreated", resourceCulture);

		internal static string StaticMethodCannotBeDispatchedToInstances => ResourceManager.GetString("StaticMethodCannotBeDispatchedToInstances", resourceCulture);

		internal FakesFrameworkResources()
		{
		}
	}
}

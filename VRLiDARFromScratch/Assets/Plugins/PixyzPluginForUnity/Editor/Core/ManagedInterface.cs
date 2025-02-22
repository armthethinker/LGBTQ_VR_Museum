﻿using UnityEngine;
using UnityEditor;

namespace Pixyz.Plugin4Unity.Native
{
    [InitializeOnLoad]
    public static partial class NativeInterface
    {
        private const string PiXYZPlugin4Unity_dll = "PiXYZPlugin4Unity";
                
        static NativeInterface()
        {
            try
            {
                Initialize(Application.isEditor && !Application.isBatchMode);   
            } 
            catch(System.Exception e)
            {
                try
                {
                    if(!CheckLicense())
                    {
                        Debug.LogWarning("The Pixyz Plugin for Unity requires a valid License.\nPlease install yours via the License Manager or visit www.pixyz-software.com to get one");
                    }
                    else
                        Debug.LogError("Exception while initializing Pixyz plugin : " + e.Message);
                    return;
                }
                catch {
                    Debug.LogError("Exception while initializing Pixyz plugin : " + e.Message);
                }
                
            }
        }
    }
}
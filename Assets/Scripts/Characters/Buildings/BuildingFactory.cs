using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Characters.Buildings
{
    public class BuildingFactory : Singletons.Singleton<BuildingFactory>
    {
        BuildingManager _bm;

        protected override void Awake()
        {
            base.Awake();
            _bm = BuildingManager.Instance;
        }

        /// <summary>
        /// 주문에 맞춰 Building Prefab을 생성한다. 
        /// </summary>
        /// <param name="buildingRequestment"></param>
        /// <returns></returns>
        public GameObject CreateBuilding(BuildingRequestment buildingRequestment)
        {
            GameObject building = _bm.Find(buildingRequestment.name);
            var slotTransform = buildingRequestment.owner.Slots[buildingRequestment.index].transform;
                
            return Instantiate(building, slotTransform.position, slotTransform.rotation, buildingRequestment.owner.transform) as GameObject;
        }

        public GameObject UpgradeBuilding(BuildingRequestment buildingRequestment) { return null; }
        public void DestroyBuilding(BuildingRequestment buildingRequestment) { }

        //    public Building CreateBuilding(BuildingRequestment buildingRequestment)
        //    {
        //        //string[] propertys = new string[3]{ "name", "buildingClass", "index"};
        //        PropertyInfo[] propertyInfos = typeof(BuildingRequestment).GetProperties();
        //        Type type = Type.GetType(buildingRequestment.name, true);

        //        // TODO :: 그래도 type이 null일 경우 error를 뿜뿜해라.
        //        if(type == null)
        //        {
        //            var currentAssembly = Assembly.GetExecutingAssembly();
        //            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        //            foreach (var assemblyName in referencedAssemblies)
        //            {
        //                var assembly = Assembly.Load(assemblyName);
        //                if (assembly != null)
        //                {
        //                    type = assembly.GetType(buildingRequestment.name);
        //                    if (type != null)
        //                        break;
        //                }
        //            }
        //        }

        //        // TODO :: Activator 알아보기
        //        object instance = Activator.CreateInstance(type);

        //        foreach(PropertyInfo property in propertyInfos)
        //        {
        //            PropertyInfo prop = type.GetProperty(property.Name);
        //            object value = buildingRequestment.GetType().GetProperty(property.Name).GetValue(buildingRequestment, null);

        //            prop.SetValue(instance, value, null);
        //        }
        //        Debug.Log(instance);
        //        Debug.Log(instance.GetType());

        //        return instance as Building;
        //    }
    }
}

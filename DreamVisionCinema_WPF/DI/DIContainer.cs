﻿using Unity;

namespace DreamVisionCinema_WPF.DI
{
    public class DIContainer
    {
        private DIContainer() { }

        private static IUnityContainer _container = null;
        public static IUnityContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
            }
            return _container;
        }
    }
}

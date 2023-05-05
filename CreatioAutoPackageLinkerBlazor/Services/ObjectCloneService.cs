namespace CreatioAutoPackageLinkerBlazor.Services {
    public static class ObjectCloneService {
        public static T Clone<T>(T originalEntity) where T : class, new() {
            T newEntity = new T();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties) {
                if (property.CanRead && property.CanWrite) {
                    property.SetValue(newEntity, property.GetValue(originalEntity));
                }
            }

            return newEntity;
        }
    }
}

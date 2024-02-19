namespace BlApi
{
    public static class Factory
    {
        /// <summary>
        /// Gets an instance of the IBl interface.
        /// </summary>
        /// <returns>An instance of the IBl interface.</returns>
        public static IBl Get() => new BlImplementation.Bl();
    }
}

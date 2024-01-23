namespace XRC.Core
{
    /// <summary>
    /// Interface to be implemented by components, such as edit tools and interaction techniques, that have a distinct toggle condition.
    /// A toggle condition is where the tool, or technique, can toggle it's state.
    /// </summary>
    /// <remarks>
    /// An example of a tool with a run condition is the XRC Color Tool, which is running when an object's color is being updated by the tool.
    /// </remarks>
    /// 
    public interface IToggle
    {
        /// <summary>
        /// Whether the tool is running or not.
        /// </summary>
        bool isOn { get; }



        /// <summary>
        /// Toggle the run condition
        /// </summary>
        public void ToggleRun();


    }
}
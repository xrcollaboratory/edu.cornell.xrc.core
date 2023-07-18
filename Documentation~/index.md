# XRC Core

## Overview

<!--  Provide a brief, high-level explanation of the package.-->
The XRC Core package is a comprehensive collection of resources, utilities, and design patterns that serve as the foundation for all XRC packages within the Unity development environment. It provides developers with a core set of tools to streamline the creation of immersive experiences and applications.

By leveraging the XRC Core package, developers can benefit from a unified and cohesive toolkit, simplifying the development process, accelerating iteration cycles, and ultimately delivering polished and immersive experiences to their end users.

## Package contents	
<!--  Include the location of important files you want the user to know about. For example, if this is a sample package containing textures, models, and materials separated by sample group, you might want to provide the folder location of each group.-->

These directories, along with their respective files, are strategically organized within the XRC Core package to provide developers with a structured and accessible resource for XR development. Navigating these directories will grant developers access to a wealth of design patterns and tools that streamline the development process and enhance the overall quality of XR experiences.


Here are some of the important files and their respective locations within the package:


### Patterns


**Singleton**

The Singleton pattern is a creational design pattern that ensures the existence of only one instance of a class and provides a global point of access to it. In Unity scripting, the Singleton pattern is commonly used to manage resources, data, or systems that should have a single, centralized instance throughout the lifespan of the application.


**Observer**

The Observer pattern is a behavioral design pattern that establishes a one-to-many relationship between objects. In this pattern, when the state of one object (called the subject) changes, all its dependent objects (called observers) are automatically notified and updated. In Unity scripting, the Observer pattern is commonly used to implement event-driven systems, allowing objects to react and respond to changes or events in a decoupled and flexible manner.

**Command**

In Unity, the Command pattern is particularly useful for implementing features like undo and redo functionality, input handling, AI behavior, and more. It helps organize and manage complex interactions by separating the requester, the controller, and the executor (command object) of an operation.


### Utilities

**PoseMarker**

PoseMarker, which assists in visualizing transforms and tracking object positions within a scene. This feature greatly facilitates the precise placement and manipulation of objects, contributing to a more efficient development process.


<!-- 
Furthermore, the package incorporates essential math classes that enable complex calculations and operations commonly required in virtual reality and augmented reality applications. These math classes encompass vector and quaternion math, projective geometry, meshes, and other useful scripts, empowering developers to implement advanced visual and spatial computations effortlessly.



Runtime - Patterns: This directory houses a comprehensive collection of game design patterns commonly employed in XR development. These patterns serve as reusable templates and methodologies that enhance code organization, maintainability, and extensibility. Developers can find a variety of patterns, such as the Singleton pattern, Observer pattern, State pattern, and more, within this directory.

Runtime - Patterns - Command: Within the Patterns directory, the Command subdirectory specifically focuses on the Command pattern. The Command pattern is particularly useful for implementing Undo/Redo command operations. By encapsulating actions or operations into separate command objects, developers can easily track and execute these commands, enabling efficient undo and redo functionality in their XR applications. -->



## Installation instructions
<!--  You can point to the official Package Manager installation instructions, but if you have any special installation requirements, such as installing samples, add them here. -->

Use the Unity Package Manager (in Unity’s top menu: 
`Window > Package Manager`) to view which packages are available for installation or already installed in your Project. In addition, you can use this window to see which versions are available, and install, remove, disable, or update packages for each Project.

To install using git url, click on the `+` icon in the top left corner of the Package Manager window. 

Then select `Add package from git URL...` and paste the following URL. 

https://github.com/xrcollaboratory/edu.cornell.xrc.core.git

From the package window, you will see the package name, it's description, and a dropdown menu for **Samples**. Import any Samples that are useful for your project for examples on how the package can be used. 



## Requirements	
<!-- This is a good place to add hardware or software requirements, including which versions of the Unity Editor this package is compatible with. -->

XRC.Core package should work with most versions of Unity. It has been tested on versions 2021.3 and above. 

<!-- 
## Limitations	
If your package has any known limitations, you can list them here. If not, or if the limitations are trivial, exclude this section.
-->


## Workflows	
<!-- Include a list of steps that the user can easily follow that demonstrates how to use the feature. You can include screenshots to help describe how to use the feature. -->

In most cases, you will not need to modify scripts within XRC.Core. Any files in the Package directory are set to Read Only and therefore should not be modified. The recommended workflow is to extend existing classes rather than modify them. This can be done by inheritance or composition. 

<!-- ## Advanced topics
This is where you can provide detailed information about what you are providing to users. This is ideal if you don’t want to overwhelm the user with too much information up front. 

## Reference
<!-- If you have a user interface with a lot of properties, you can provide the details in a reference section. Using tables is a good way to provide quick access to specific property descriptions. -->

## Samples
<!-- For packages that include sample files, you can include detailed information on how the user can use these sample files in their projects and scenes.-->

**StarterAssets**

The Starter Assets that offer a range of pre-built components, materials, models, prefabs, and scenes. These assets provide a solid starting point for building virtual environments, saving developers valuable time and effort in the initial setup process.

<!--
## Tutorials
If you want to provide walkthroughs for complicated procedures, you can also add them here. Use step-by-step instructions and include images if they can help the user understand.
-->
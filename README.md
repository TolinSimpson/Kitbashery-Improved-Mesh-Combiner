# Notice:
This repo was for the UV tools created for the Kitbashery standalone application; a project which was canceled. This repo uses the name Kitbashery but is not offcially part of the Kitbashery ecosystem and does not meet the quality standards of other Kitbashery assets. If you want this and more UV tools that I have since developed give me a shout @ https://github.com/Kitbashery and maybe I'll clean this up as a modern Unity package.

# Kitbashery-Improved-Mesh-Combiner
An improved mesh combiner developed for Kitbashery. This contains an alternative implementation of Unity's built-in mesh combiner.
<br>

<h1>License:</h1>
<b>MIT License (see Licensing for more info)</b>
<br>
Created by Jorge Reyna Tamez @ https://www.fiverr.com/wkepro
<br>
Maintained by Kitbashery @ https://www.kitbashery.com
<br>
<i>The name Kitbashery and all associated branding and logos are are under copyright ©2020-22 Tolin Simpson all rights reserved.</i>
<br>

<h1>Features:</h1>
This project contains an improved mesh combiner for use in Kitbashery it does the following:
<br>
<ul>
 <li>Combines Meshes into a single mesh. </li>
 <li>(optionally) Merges vertices into vertex welds. </li>
 <li>Packs UVs so they don't overlap or stretch (includes padding & size by bounds options)</li>
  </ul>
  
This does <b>NOT</b> do the following (Kitbashery doesn't need these features):

<ul>
  <li>Pack textures.</li>
  <li>Combine submeshes.</li>
  <li>Combine skinned meshes.</li>
  <li>Preserve vertex colors.</li>
  </ul>

<h1>Usage:</h1>
<br>
1) Import the .unitypackage file found under releases.
<br>
2) Editor implementation can be found under <i>Tools>Kitbashery Combine Meshes></i>
<br>
<br>
<b>Testing:</b>
Kitbashery contains a modified version of https://github.com/Chaser324/unity-wireframe" (required dependancy) wireframe shader that has been modified to display the wireframe of UVs in worldspace grab it from here:
https://github.com/kitbashery/Kitbashery/blob/Latest-Experimental/Assets/_Kitbashery/Shaders/UVWireframeShaded-Unlit.shader"
<br>
<br>
<i>Note: Unity's default quad will always be a perfect UV square and represent UV bound in worldspace. Also note that all UVs are offset by 0.5f in X and Y direction when visualized.</i>

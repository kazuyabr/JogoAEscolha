Thanks for purchasing Bitmap2Material 3 for Unity!

Bitmap2Material 3 allows you to create a PBR Material from a single diffuse.

Bitmap2Material 3 for Unity contains 3 Substance filters:
- Bitmap2Material_3-X.sbsar: this filter can be used within Unity and other compatible Substance integrations to create a full PBR material
- Bitmap2Material_2.sbsar: this filter should be used if you wish to create non-PBR materials
- Bitmap2Material_2_RealTimeMobile.sbsar : this filter should be used if you wish to create non-PBR materials for mobile
- Bitmap2Material_3.sbsar: legacy version of the filter compatible with integrations that use a previous version of Substance Engine

IMPORTANT NOTICE: 
It's not recommended to replace an existing version of the filter in a project as it can create inconsistency between the old and new version of the filter.
Bitmap2Material 3 can be used by both PBR mode: Standard (Metallic/Smoothness=Glossiness) and Standard Specular setup (Specular/Smoothness=Glossiness). 
Be use to enable the corresponding outputs (by clicking on the sbsar in Unity "Project" view) using the checkbox options at the bottom of the parameters.
Unity will automatically pack the outputs correctly to fit Unity PBR shaders convention depending on the current shader selection.

Here are the steps to use B2M3 in Unity:
1) Drag&Drop the substance called "Bitmap2Material_3-X" on a mesh
2) Select a bitmap in "Main Input" slot of the Substance
3) Select the shader called «Standard» or «Standard (Specular setup)»
You can also create a separate material and use the "Bitmap2Material_3" outputs in the corresponding slots.

It's possible to use the sbsar filters as "standalone" tools to generate bitmaps by using Substance Player (free application).

If you need more information:
Online documentation: http://support.allegorithmic.com
Contact Form: http://www.allegorithmic.com/contact

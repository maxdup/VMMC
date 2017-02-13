Valve Manifest Collapser (VMMC)
===========================

Collapses a VMM and its instances into a single VMF.

Why Use Vmmc
------------

VMM files are not very well supported by VBsp. To use these Hammer features, we need to change the files we give to vbsp into something it can process.

Here's the issues that VMMC aims to fix:
1. Compilling a VMM file with Vbsp results in an empty worldspawn, this means information gets lost (skybox texture names, detail sprites etc...)
2. Instances contained in VMM submaps cannot be compiled. VBsp don't agree on where instanced vmfs should be. VBsp is looking for instanced vmfs relative to the vmm, Hammer loads instances relative to their submaps.
3. Instanced displacements will get their offset messed up by vbsp.

VMMC fixes all of this by collapsing everything in a single file, making the task easier on vbsp.


How to Use
----------

VMMC is a command line utility

You want to run VMMC.exe with a path to your vmm as parameters

	vmmc C:\mapsrc\example.vmm
	
vmmc will output to C:\mapsrc\example.vmf


What needs to be implemented
----------------------------

- Instanced info_overlays will often get jumbled. They work fine in submaps however.



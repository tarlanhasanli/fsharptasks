(*
   Define predicates

   fsTreeWf : FsTree -> bool

   pathWf   : string list -> bool

   that check whether the given tree is well-formed as a filesystem
   and whether the path (represented as a list of strings) is well-formed.

   A well-formed filesystem cannot have identical paths leading to
   different nodes in the tree.

   A well-formed path cannot contain nodes with empty names. 

   FsTree is a tree data type that we think of as a filesystem.
   It is peculiar since there is no difference between files and direcotries,
   everything is a node.
*)

(*
   Define a FsCheck property

   createDirWf : string list -> FsTree -> Property

   which checks that creating a node in a well-formed tree (filesystem)
   results in a wellformed tree.

   Define this using a conditional property (==>).

   Convince yourself that this is a bad way to do testing by observing
   the amount of test inputs that trivially satisfy this property.
*)

(*
   Define a generator

   wfTree : Gen<FsTree>

   that generates only well-formed trees (filesystems).


   Define a generator

   wfPath : Gen<string list>

   that generates only well-formed filesystem paths.


   You may want to use the predicates defined above to check that
   the generated data indeed is well-formed.
*)

(*
   Define an FsCheck property

   deleteIsWellFormed : string list -> FsTree -> bool

   which checks that given
   p  : string list
   fs : FsTree
   we have that after deleting p from fs the result is well-formed.

   You may assume here that this property is only used with the
   "well-formed" generators.

   The correct behaviour of delete is that if p is not present in fs
   then fs is returned as is.   
*)

(*
   Define an FsCheck property

   createDirExists : string list -> FsTree -> bool

   which checks that given
   p  : string list
   fs : FsTree
   we have that the path p is included (exactly once) in the
   result of show after we have created the directory p in fs.

   Here you may assume that this property is used only
   with well-formed generators, i.e., the fs and p that
   are passed in as parameters satisfy the well-formedness condition.

   The correct behaviour of createDir p fs is that it returns
   the given fs if p already exists (as a directory) in fs.
*)

(*
   Define an FsCheck property

   deleteDeletes : FsTree -> bool

   which checks that given an
   fs : FsTree
   we have that by deleting one by one all of the items in the result of
   show fs we end up with an empty filesystem.
   
*)

(*
   Define an FsCheck property

   createAndDelete : FsTree -> string list -> string list -> Property

   which checks that given
   
   fs : FsTree
   p1 : string list
   p2 : string list

   we have that if p1 is not a prefix of p2 then

   1) creating directory p1 in fs
   2) creating directory p2 in the result
   3) deleting p1 from the result

   gives a filesystem which still contains p2.
*)
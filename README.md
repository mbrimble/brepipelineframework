# brepipelineframework
#Project Description
The BizTalk BRE Pipeline Framework leverages the Business Rules Engine (BRE) to abstract away logic to be exercised in BizTalk pipelines thus catering for increased development flexibility and agility, especially when dealing with rapidly changing requirements, and easier deployments.  Note that this framework currently only works with BizTalk Server 2010, 2013, and 2013 R2.
The design goals of the BRE Pipeline Framework are as below
Abstract pipeline component logic into the BRE, allowing for logic to be changed at runtime without having to redeploy BizTalk artefacts. 
Provide a collection of reusable common functions that are exercised in pipellines so that developers can concentrate on logic rather than plumbing.  Said functions will be encapsulated by business analyst friendly BRE vocabularies making for very readable rules. 
Provide an extensible framework that allows for developers to implement their relevant custom requirements if not catered for out of the box in the desired manner. 
Provide a means for developers to easily understand rules execution logic by providing appropriate debug tracing information when required. 
The BRE Pipeline Framework allows you to inspect and manipulate BizTalk message content and context within a pipeline.  It allows you to make use of XML and SQL based BRE vocabularies as you would normally use them, provides a collection of .NET based BRE vocabularies which should satisfy most of your message content and context assessment/manipulation requirements, and allows you to create custom .NET classes and corresponding vocabularies to extend the framework to fulfill your custom requirements. 

See the documentation page for more details on how to install, use and extend the BRE Pipeline Framework.

﻿Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.Testing.Verifiers

Public Class CodefixVerifier(Of TAnalyzer As {DiagnosticAnalyzer, New}, TCodefix As {CodeFixProvider, New})
    Inherits VisualBasicCodeFixVerifier(Of TAnalyzer, TCodefix, XUnitVerifier)
End Class

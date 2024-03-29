﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Navigation;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Text;
using System.Threading;

namespace RoslynPad.Roslyn.Navigation
{
    [ExportWorkspaceService(typeof(IDocumentNavigationService))]
    internal sealed class DocumentNavigationService : IDocumentNavigationService
    {
        //public bool CanNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan)
        //{
        //    return true;
        //}

        //public bool CanNavigateToLineAndOffset(Workspace workspace, DocumentId documentId, int lineNumber, int offset)
        //{
        //    return true;
        //}

        //public bool CanNavigateToPosition(Workspace workspace, DocumentId documentId, int position, int virtualSpace = 0)
        //{
        //    return true;
        //}

        //public bool TryNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan, OptionSet? options = null)
        //{
        //    return true;
        //}

        //public bool TryNavigateToLineAndOffset(Workspace workspace, DocumentId documentId, int lineNumber, int offset,
        //    OptionSet? options = null)
        //{
        //    return true;
        //}

        //public bool TryNavigateToPosition(Workspace workspace, DocumentId documentId, int position, int virtualSpace = 0,
        //    OptionSet? options = null)
        //{
        //    return true;
        //}

        public bool CanNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan, CancellationToken cancellationToken)
        {
            return true;
        }

        public bool CanNavigateToLineAndOffset(Workspace workspace, DocumentId documentId, int lineNumber, int offset, CancellationToken cancellationToken)
        {
            return true;
        }

        public bool CanNavigateToPosition(Workspace workspace, DocumentId documentId, int position, int virtualSpace, CancellationToken cancellationToken)
        {
            return true;
        }

        public bool TryNavigateToSpan(Workspace workspace, DocumentId documentId, TextSpan textSpan, OptionSet options, bool allowInvalidSpan, CancellationToken cancellationToken)
        {
            return true;
        }

        public bool TryNavigateToLineAndOffset(Workspace workspace, DocumentId documentId, int lineNumber, int offset, OptionSet options, CancellationToken cancellationToken)
        {
            return true;
        }

        public bool TryNavigateToPosition(Workspace workspace, DocumentId documentId, int position, int virtualSpace, OptionSet options, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
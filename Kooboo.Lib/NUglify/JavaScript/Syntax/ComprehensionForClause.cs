﻿// ComprehensionForClause.cs
//
// Copyright 2013 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using NUglify.JavaScript.Visitors;

namespace NUglify.JavaScript.Syntax
{
    public class ComprehensionForClause : ComprehensionClause
    {
        private AstNode m_binding;
        private AstNode m_expression;

        public AstNode Binding
        {
            get { return m_binding; }
            set
            {
                ReplaceNode(ref m_binding, value);
            }
        }

        public bool IsInOperation { get; set; }

        public SourceContext OfContext { get; set; }

        public AstNode Expression
        {
            get { return m_expression; }
            set
            {
                ReplaceNode(ref m_expression, value);
            }
        }

        public ComprehensionForClause(SourceContext context)
            : base(context)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            if (visitor != null)
            {
                visitor.Visit(this);
            }
        }

        public override IEnumerable<AstNode> Children
        {
            get
            {
                return EnumerateNonNullNodes(Binding, Expression);
            }
        }

        public override bool ReplaceChild(AstNode oldNode, AstNode newNode)
        {
            if (Binding == oldNode)
            {
                Binding = newNode;
                return true;
            }

            if (Expression == oldNode)
            {
                Expression = newNode;
                return true;
            }

            return false;
        }
    }
}

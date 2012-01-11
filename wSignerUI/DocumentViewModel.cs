﻿using System.ComponentModel;
using System.IO;
using System.Linq;

namespace wSignerUI
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string propName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }

        private string[] _docsToSign;
        public string[] DocsToSign
        {
            get { return _docsToSign; }
            set 
            { 
                _docsToSign = value;
                FirePropertyChanged("StatusText");
                FirePropertyChanged("HasNotSelected");
                FirePropertyChanged("HasSelected");
                FirePropertyChanged("IsPDF");
                FirePropertyChanged("IsOOXML");
                FirePropertyChanged("IsBoth");
                FirePropertyChanged("IsNeither");
                FirePropertyChanged("Count");
            }

        }

        public string StatusText
        {
            get
            {
                return HasSelected
                        ? IsNeither
                           ? "Only PDF and Microsoft Office 2007 documents (docx, xlsx, pptx) are supported at the moment."
                           : Count > 1
                                ? "Sign " + Path.GetFileName(_docsToSign[0])
                                : "Sign " + _docsToSign.Length + "documents"
                        : "Drop documents into this box to sign with your digital signature. (Supports pdf, docx, xlsx, pptx)";
            }
        }

        public bool IsPDF
        {
            get
            {
                return DocsToSign!=null && DocsToSign.Any(f => f != null && f.EndsWith(".pdf", System.StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public bool IsOOXML
        {
            get
            {
                return DocsToSign!=null &&  DocsToSign.Any(f => f != null && (
                    f.EndsWith(".xlsx", System.StringComparison.InvariantCultureIgnoreCase)
                    || f.EndsWith(".pptx", System.StringComparison.InvariantCultureIgnoreCase)
                    || f.EndsWith("docx", System.StringComparison.InvariantCultureIgnoreCase)));
            }
        }

        public bool HasNotSelected
        {
            get { return DocsToSign == null; }
        }

        public bool HasSelected
        {
            get { return DocsToSign != null; }
        }

        public bool IsBoth
        {
            get { return HasSelected && (IsPDF && IsOOXML); }
        }

        public bool IsNeither { get { return HasSelected && !(IsPDF || IsOOXML); } }

        public int Count
        {
            get { return HasSelected ? DocsToSign.Length : 0; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Comtech.Wbxml
{
	public class WbxmlWriter : BaseWbxmlWriter
	{
		[Flags]
		public enum ElementFlags { HasAttributes = 1, HasChildren = 2 };

		public WbxmlWriter(Stream s, Encoding encoding)
			:
			base(s, encoding)
		{
		}

		public void WriteDocument(RootElement root)
		{
			base.WriteHeader();
			WriteElement(root);
		}

		public void WriteElement(Element element)
		{
			ElementFlags flags = 0;
			if (element.HasAttributes)
				flags |= ElementFlags.HasAttributes;
			if (element.HasChildren)
				flags |= ElementFlags.HasChildren;
			WriteElement(element.Tag, flags);
			if (element.HasAttributes)
			{
				foreach (Attribute attr in element.Attributes.Keys)
				{
					WriteAttribute(attr, element[attr]);
				}
				WriteTerminator();
			}
			if (element.HasChildren)
			{
				foreach (Element child in element.Children)
				{
					WriteElement(child);
				}
				WriteTerminator();
			}
		}

		public void WriteAttribute(Attribute attr, object value)
		{
			base.WriteAttribute((byte)attr, value);
		}

		public void WriteElement(Tag tag, ElementFlags flags)
		{
			byte token = (byte)tag;
			if ((flags & ElementFlags.HasAttributes) != 0)
				token |= 0x80;
			if ((flags & ElementFlags.HasChildren) != 0)
				token |= 0x40;
			base.WriteElement(token);
		}

		/// <summary>
		/// Writes an "fld" tag having the specified id and val attributes.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		public void WriteFldElement(string id, object value)
		{
			if (value == null)
				return;
			WriteElement(Tag.Fld, ElementFlags.HasAttributes);
			WriteAttribute(Attribute.Id, id);
			WriteAttribute(Attribute.Val, value);
			WriteTerminator();
		}

		/// <summary>
		/// Writes a "row" tag containing as many "fld" child elements as in fieldNames/values.
		/// The lengths of fieldNames and values must be the same.
		/// </summary>
		/// <param name="fieldNames"></param>
		/// <param name="values"></param>
		public void WriteRowElement(string[] fieldNames, params object[] values)
		{
			WriteElement(Tag.Row, ElementFlags.HasChildren);
			for (int i = 0; i < fieldNames.Length; i++)
			{
				WriteFldElement(fieldNames[i], values[i]);
			}
			WriteTerminator();
		}

		public void WriteRowElementWithFormat(string[] fieldNames, string[] fieldFormats, params object[] values)
		{
			WriteElement(Tag.Row, ElementFlags.HasChildren);
			for (int i = 0; i < fieldNames.Length; i++)
			{
				object value = values[i];
				string format = fieldFormats[i];
				WriteFldElement(fieldNames[i],
					value == null ? null :
					format == null ? value :
					String.Format(format, value));
			}
			WriteTerminator();
		}

		/// <summary>
		/// Writes a "row" tag (without child elements) containing as many attributes as in attributes/values.
		/// The lengths of attributes and values must be the same.
		/// </summary>
		/// <param name="fieldNames"></param>
		/// <param name="values"></param>
		public void WriteRowElementWithAttributes(Attribute[] attributes, params object[] values)
		{
			WriteElement(Tag.Row, ElementFlags.HasAttributes);
			for (int i = 0; i < attributes.Length; i++)
			{
				WriteAttribute(attributes[i], values[i]);
			}
			WriteTerminator();
		}

		public void WriteLstElement(string listName)
		{
			WriteElement(Comtech.Wbxml.Tag.Lst, WbxmlWriter.ElementFlags.HasAttributes | WbxmlWriter.ElementFlags.HasChildren);
			WriteAttribute(Comtech.Wbxml.Attribute.Id, listName);
			WriteTerminator(); // end of attributes
		}

		public void WriteLstElement(string listName, string[] fieldNames, string[][] rows)
		{
			WriteLstElement(listName);

			foreach (string[] rowValues in rows)
				WriteRowElement(fieldNames, rowValues);

			WriteTerminator(); // end of lst
		}

		public void WriteLstElement(string listName, string[] fieldNames, string[] fieldFormats, System.Collections.IEnumerable rows)
		{
			WriteLstElement(listName);

			foreach (object[] rowValues in rows)
				WriteRowElementWithFormat(fieldNames, fieldFormats, rowValues);

			WriteTerminator(); // end of lst
		}

	}
}

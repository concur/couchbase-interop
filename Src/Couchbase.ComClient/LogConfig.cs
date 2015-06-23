using System;
using System.Runtime.InteropServices;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Couchbase.ComClient
{
	[Guid("46F8E1EC-9AA0-491D-8126-01C7454C549B")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface ILogConfig
	{
		[DispId(1)]
		long ArchiveAboveSize { get; set; }
		[DispId(2)]
		string ArchiveEvery { get; set; }
		[DispId(3)]
		string ArchiveNumbering { get; set; }
		[DispId(5)]
		bool DeleteOldFileOnStartup { get; set; }
		[DispId(5)]
		bool EnableFileDelete { get; set; }
		[DispId(6)]
		string Encoding { get; set; }
		[DispId(7)]
		string FileName { get; set; }
		[DispId(8)]
		string Footer { get; set; }
		[DispId(9)]
		string Header { get; set; }
		[DispId(10)]
		string Layout { get; set; }
		[DispId(11)]
		int MaxArchiveFiles { get; set; }
		[DispId(12)]
		string MinLogLevel { get; set; }
		[DispId(13)]
		void SetupLogging();
	}

	[Guid("3E14F6C5-3E39-4C54-9E1A-6F4C0BCCEF39")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("Couchbase.ComClient.LogConfig")]
	public class LogConfig : ILogConfig
	{
		public LogConfig()
		{
			m_minLogLevel = LogLevel.Info;
			m_fileLog = new FileTarget();
			m_fileLog.FileName = "Couchbase.ComClient.txt";
		}

		private LogLevel m_minLogLevel;
		public string MinLogLevel
		{
			get { return m_minLogLevel.Name; }
			set { m_minLogLevel = LogLevel.FromString(value); }
		}

		FileTarget m_fileLog;
		public string FileName
		{
			get { return m_fileLog.FileName != null ? m_fileLog.FileName.ToString() : null; }
			set { m_fileLog.FileName = value; }
		}
		public string Layout
		{
			get { return m_fileLog.Layout != null ? m_fileLog.Layout.ToString() : null; }
			set { m_fileLog.Layout = value; }
		}
		public string Header
		{
			get { return m_fileLog.Header != null ? m_fileLog.Header.ToString() : null; }
			set { m_fileLog.Header = value; }
		}
		public string Footer
		{
			get { return m_fileLog.Footer != null ? m_fileLog.Footer.ToString() : null; }
			set { m_fileLog.Footer = value; }
		}
		public string ArchiveEvery
		{
			get { return m_fileLog.ArchiveEvery.ToString(); }
			set { m_fileLog.ArchiveEvery = (FileArchivePeriod)Enum.Parse(typeof(FileArchivePeriod), value); }
		}
		public int MaxArchiveFiles
		{
			get { return m_fileLog.MaxArchiveFiles; }
			set { m_fileLog.MaxArchiveFiles = value; }
		}
		public long ArchiveAboveSize
		{
			get { return m_fileLog.ArchiveAboveSize; }
			set { m_fileLog.ArchiveAboveSize = value; }
		}
		public string ArchiveNumbering
		{
			get { return m_fileLog.ArchiveNumbering.ToString(); }
			set { m_fileLog.ArchiveNumbering = (ArchiveNumberingMode)Enum.Parse(typeof(ArchiveNumberingMode), value); }
		}
		public bool DeleteOldFileOnStartup
		{
			get { return m_fileLog.DeleteOldFileOnStartup; }
			set { m_fileLog.DeleteOldFileOnStartup = value; }
		}
		public bool EnableFileDelete
		{
			get { return m_fileLog.EnableFileDelete; }
			set { m_fileLog.EnableFileDelete = value; }
		}
		public string Encoding
		{
			get { return m_fileLog.Encoding.WebName; }
			set { m_fileLog.Encoding = System.Text.Encoding.GetEncoding(value); }
		}

		public void SetupLogging()
		{
			Common.Logging.LogManager.Adapter = new Common.Logging.NLog.NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);

			LoggingConfiguration config = new LoggingConfiguration();
			config.AddTarget("file", m_fileLog);
			var rule = new LoggingRule("*", m_minLogLevel, m_fileLog);
			config.LoggingRules.Add(rule);
			LogManager.Configuration = config;
		}
	}
}

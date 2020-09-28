using Kogane.DebugMenu.Internal;
using System;
using System.Linq;
using System.Text;

namespace Kogane.DebugMenu
{
	/// <summary>
	/// プロファイラ情報を表示するクラス
	/// </summary>
	public sealed class ProfilerInfoListCreator : ListCreatorBase<ActionData>
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly MonoMemoryChecker  m_monoMemoryChecker  = new MonoMemoryChecker();
		private readonly UnityMemoryChecker m_unityMemoryChecker = new UnityMemoryChecker();

		//==============================================================================
		// 変数
		//==============================================================================
		private ActionData[] m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int Count => m_list.Length;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			m_list = ToText()
					.Split( '\n' )
					.Where( x => data.IsMatch( x ) )
					.Select( x => new ActionData( x ) )
					.ToArray()
				;

			if ( data.IsReverse )
			{
				Array.Reverse( m_list );
			}
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override ActionData DoGetElemData( int index )
		{
			return m_list.ElementAtOrDefault( index );
		}

		/// <summary>
		/// テキストを整形して返します
		/// </summary>
		private string ToText()
		{
			m_monoMemoryChecker.Update();
			m_unityMemoryChecker.Update();

			var builder = new StringBuilder();

			builder.AppendLine( "<b>Mono</b>" );
			builder.AppendLine( "" );
			builder.AppendLine( $"    Used: {m_monoMemoryChecker.UsedText}" );
			builder.AppendLine( $"    Total: {m_monoMemoryChecker.TotalText}" );
			builder.AppendLine( "" );
			builder.AppendLine( "<b>Unity</b>" );
			builder.AppendLine( "" );
			builder.AppendLine( $"    Used: {m_unityMemoryChecker.UsedText}" );
			builder.AppendLine( $"    Unused: {m_unityMemoryChecker.UnusedText}" );
			builder.AppendLine( $"    Total: {m_unityMemoryChecker.TotalText}" );
			builder.AppendLine( "" );

			return builder.ToString();
		}
	}
}
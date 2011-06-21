///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace Sharplike.UI
{
	public partial class GlyphMap
	{
		public static GlyphMap LoadDefaults(GlyphMap glyphMap)
		{
			String[] names = Enum.GetNames(typeof(GlyphDefault));

			glyphMap.Clear();

			foreach (String name in names)
			{
				Object o = Enum.Parse(typeof(GlyphDefault), name);
				glyphMap.Register((GlyphDefault)o, (Int32)o);
			}

			glyphMap['!'] = (Int32)GlyphDefault.Exclamation;
			glyphMap['"'] = (Int32)GlyphDefault.DoubleQuote;
			glyphMap['#'] = (Int32)GlyphDefault.Hash;
			glyphMap['$'] = (Int32)GlyphDefault.Dollar;
			glyphMap['%'] = (Int32)GlyphDefault.Percent;
			glyphMap['&'] = (Int32)GlyphDefault.Ampersand;
			glyphMap['\''] = (Int32)GlyphDefault.Apostrophe;
			glyphMap['('] = (Int32)GlyphDefault.LeftParentheses;
			glyphMap[')'] = (Int32)GlyphDefault.RightParentheses;
			glyphMap['*'] = (Int32)GlyphDefault.Asterisk;
			glyphMap['+'] = (Int32)GlyphDefault.Plus;
			glyphMap[','] = (Int32)GlyphDefault.Comma;
			glyphMap['-'] = (Int32)GlyphDefault.Hyphen;
			glyphMap['.'] = (Int32)GlyphDefault.Period;
			glyphMap['/'] = (Int32)GlyphDefault.Foreslash;
			
			glyphMap['0'] = (Int32)GlyphDefault.Zero;
			glyphMap['1'] = (Int32)GlyphDefault.One;
			glyphMap['2'] = (Int32)GlyphDefault.Two;
			glyphMap['3'] = (Int32)GlyphDefault.Three;
			glyphMap['4'] = (Int32)GlyphDefault.Four;
			glyphMap['5'] = (Int32)GlyphDefault.Five;
			glyphMap['6'] = (Int32)GlyphDefault.Six;
			glyphMap['7'] = (Int32)GlyphDefault.Seven;
			glyphMap['8'] = (Int32)GlyphDefault.Eight;
			glyphMap['9'] = (Int32)GlyphDefault.Nine;
			glyphMap[0] = (Int32)GlyphDefault.Zero;
			glyphMap[1] = (Int32)GlyphDefault.One;
			glyphMap[2] = (Int32)GlyphDefault.Two;
			glyphMap[3] = (Int32)GlyphDefault.Three;
			glyphMap[4] = (Int32)GlyphDefault.Four;
			glyphMap[5] = (Int32)GlyphDefault.Five;
			glyphMap[6] = (Int32)GlyphDefault.Six;
			glyphMap[7] = (Int32)GlyphDefault.Seven;
			glyphMap[8] = (Int32)GlyphDefault.Eight;
			glyphMap[9] = (Int32)GlyphDefault.Nine;

			glyphMap[':'] = (Int32)GlyphDefault.Colon;
			glyphMap[';'] = (Int32)GlyphDefault.Semicolon;
			glyphMap['<'] = (Int32)GlyphDefault.LeftAngleBracket;
			glyphMap['='] = (Int32)GlyphDefault.Equals;
			glyphMap['>'] = (Int32)GlyphDefault.RightAngleBracket;
			glyphMap['?'] = (Int32)GlyphDefault.QuestionMark;
			glyphMap['@'] = (Int32)GlyphDefault.At;

			glyphMap['A'] = (Int32)GlyphDefault.A;
			glyphMap['B'] = (Int32)GlyphDefault.B;
			glyphMap['C'] = (Int32)GlyphDefault.C;
			glyphMap['D'] = (Int32)GlyphDefault.D;
			glyphMap['E'] = (Int32)GlyphDefault.E;
			glyphMap['F'] = (Int32)GlyphDefault.F;
			glyphMap['G'] = (Int32)GlyphDefault.G;
			glyphMap['H'] = (Int32)GlyphDefault.H;
			glyphMap['I'] = (Int32)GlyphDefault.I;
			glyphMap['J'] = (Int32)GlyphDefault.J;
			glyphMap['K'] = (Int32)GlyphDefault.K;
			glyphMap['L'] = (Int32)GlyphDefault.L;
			glyphMap['M'] = (Int32)GlyphDefault.M;
			glyphMap['N'] = (Int32)GlyphDefault.N;
			glyphMap['O'] = (Int32)GlyphDefault.O;
			glyphMap['P'] = (Int32)GlyphDefault.P;
			glyphMap['Q'] = (Int32)GlyphDefault.Q;
			glyphMap['R'] = (Int32)GlyphDefault.R;
			glyphMap['S'] = (Int32)GlyphDefault.S;
			glyphMap['T'] = (Int32)GlyphDefault.T;
			glyphMap['U'] = (Int32)GlyphDefault.U;
			glyphMap['V'] = (Int32)GlyphDefault.V;
			glyphMap['W'] = (Int32)GlyphDefault.W;
			glyphMap['X'] = (Int32)GlyphDefault.X;
			glyphMap['Y'] = (Int32)GlyphDefault.Y;
			glyphMap['Z'] = (Int32)GlyphDefault.Z;

			glyphMap['['] = (Int32)GlyphDefault.LeftSquareBracket;
			glyphMap['\\'] = (Int32)GlyphDefault.Backslash;
			glyphMap[']'] = (Int32)GlyphDefault.RightSquareBracket;
			glyphMap['^'] = (Int32)GlyphDefault.Caret;
			glyphMap['_'] = (Int32)GlyphDefault.Underbar;

			glyphMap['`'] = (Int32)GlyphDefault.Backtick;

			glyphMap['a'] = (Int32)GlyphDefault.a;
			glyphMap['b'] = (Int32)GlyphDefault.b;
			glyphMap['c'] = (Int32)GlyphDefault.c;
			glyphMap['d'] = (Int32)GlyphDefault.d;
			glyphMap['e'] = (Int32)GlyphDefault.e;
			glyphMap['f'] = (Int32)GlyphDefault.f;
			glyphMap['g'] = (Int32)GlyphDefault.g;
			glyphMap['h'] = (Int32)GlyphDefault.h;
			glyphMap['i'] = (Int32)GlyphDefault.i;
			glyphMap['j'] = (Int32)GlyphDefault.j;
			glyphMap['k'] = (Int32)GlyphDefault.k;
			glyphMap['l'] = (Int32)GlyphDefault.l;
			glyphMap['m'] = (Int32)GlyphDefault.m;
			glyphMap['n'] = (Int32)GlyphDefault.n;
			glyphMap['o'] = (Int32)GlyphDefault.o;
			glyphMap['p'] = (Int32)GlyphDefault.p;
			glyphMap['q'] = (Int32)GlyphDefault.q;
			glyphMap['r'] = (Int32)GlyphDefault.r;
			glyphMap['s'] = (Int32)GlyphDefault.s;
			glyphMap['t'] = (Int32)GlyphDefault.t;
			glyphMap['u'] = (Int32)GlyphDefault.u;
			glyphMap['v'] = (Int32)GlyphDefault.v;
			glyphMap['w'] = (Int32)GlyphDefault.w;
			glyphMap['x'] = (Int32)GlyphDefault.x;
			glyphMap['y'] = (Int32)GlyphDefault.y;
			glyphMap['z'] = (Int32)GlyphDefault.z;

			glyphMap['{'] = (Int32)GlyphDefault.LeftCurlyBracket;
			glyphMap['|'] = (Int32)GlyphDefault.Pipe;
			glyphMap['}'] = (Int32)GlyphDefault.RightCurlyBracket;
			glyphMap['~'] = (Int32)GlyphDefault.Tilde;


			return glyphMap;
		}
	}
}

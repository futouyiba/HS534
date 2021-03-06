/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
    internal class Curve25519
        : AbstractFpCurve
    {
        public static readonly BigInteger q = Nat256.ToBigInteger(Curve25519Field.P);

        private const int Curve25519_DEFAULT_COORDS = COORD_JACOBIAN_MODIFIED;

        protected readonly Curve25519Point m_infinity;

        public Curve25519()
            : base(q)
        {
            this.m_infinity = new Curve25519Point(this, null, null);

            this.m_a = FromBigInteger(new BigInteger(1,
                Hex.Decode("2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA984914A144")));
            this.m_b = FromBigInteger(new BigInteger(1,
                Hex.Decode("7B425ED097B425ED097B425ED097B425ED097B425ED097B4260B5E9C7710C864")));
            this.m_order = new BigInteger(1, Hex.Decode("1000000000000000000000000000000014DEF9DEA2F79CD65812631A5CF5D3ED"));
            this.m_cofactor = BigInteger.ValueOf(8);
            this.m_coord = Curve25519_DEFAULT_COORDS;
        }

        protected override ECCurve CloneCurve()
        {
            return new Curve25519();
        }

        public override bool SupportsCoordinateSystem(int coord)
        {
            switch (coord)
            {
            case COORD_JACOBIAN_MODIFIED:
                return true;
            default:
                return false;
            }
        }

        public virtual BigInteger Q
        {
            get { return q; }
        }

        public override ECPoint Infinity
        {
            get { return m_infinity; }
        }

        public override int FieldSize
        {
            get { return q.BitLength; }
        }

        public override ECFieldElement FromBigInteger(BigInteger x)
        {
            return new Curve25519FieldElement(x);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
        {
            return new Curve25519Point(this, x, y, withCompression);
        }

        protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
        {
            return new Curve25519Point(this, x, y, zs, withCompression);
        }
    }
}

#endif

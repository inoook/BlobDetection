using UnityEngine;
using System.Collections;

namespace BlobDetectionNS
{
	//==================================================
	//class EdgeDetection
	//==================================================
	public class EdgeDetection : Metaballs2D
	{
		public static byte C_R = 0x01;
		public static byte C_G = 0x02;
		public static byte C_B = 0x04;
		//public final static byte C_ALL = C_R|C_G|C_B;
	
		//public	byte	colorFlag;
		public 	int 	imgWidth, imgHeight;
		public 	byte[]	pixels;
		public	bool	posDiscrimination;
		public float	m_coeff = 3.0f * 255.0f;
	
		//--------------------------------------------
		// Constructor
		//--------------------------------------------
		public EdgeDetection (int imgWidth, int imgHeight)
		{
			this.imgWidth = imgWidth;
			this.imgHeight = imgHeight;
			base.init (imgWidth, imgHeight);

			//colorFlag=C_ALL;
			posDiscrimination = false;
		} 

		//--------------------------------------------
		// setPosDiscrimination()
		//--------------------------------------------
		public void setPosDiscrimination (bool isV)
		{
			posDiscrimination = isV;
		}

		//--------------------------------------------
		// setThreshold()
		//--------------------------------------------
		public void setThreshold (float value)
		{
			if (value < 0.0f)
				value = 0.0f;
			if (value > 1.0f)
				value = 1.0f;
			setIsovalue (value * m_coeff);
		}

		//--------------------------------------------
		// setComponent()
		//--------------------------------------------
		/*
		public void setComponent(byte flag)
		{
			if (flag==0) flag = C_ALL;
			colorFlag = flag;
		}
		*/

		//--------------------------------------------
		// setImage()
		//--------------------------------------------
		public void setImage (byte[] pixels)
		{
			this.pixels = pixels;
		}

		//--------------------------------------------
		// computeEdges()
		//--------------------------------------------
		public void computeEdges (byte[] pixels)
		{
			setImage (pixels);
			computeMesh ();	
		}

		//--------------------------------------------
		// computeIsovalue()
		//--------------------------------------------
		public override void computeIsovalue ()
		{
			int r, g, b;
			int x, y;
			int offset;
	 
			r = 0;
			g = 0;
			b = 0;
			for (y=0; y<imgHeight; y++)
				for (x=0; x<imgWidth; x++) {
					offset = x + imgWidth * y;
	   	
					// Add R,G,B
					r = pixels [offset * 4 + 0];
					g = pixels [offset * 4 + 1];
					b = pixels [offset * 4 + 2];
	   	
					gridValue [offset] = (float)(r + g + b);// /m_coeff   
				}
		}

		//--------------------------------------------
		// getSquareIndex()
		//--------------------------------------------
		protected override int getSquareIndex (int x, int y)
		{
			int squareIndex = 0;
			int offy = resx * y;
			int offy1 = resx * (y + 1);
     
			if (posDiscrimination == false) {
				if (gridValue [x + offy] < isovalue)
					squareIndex |= 1;
				if (gridValue [x + 1 + offy] < isovalue)
					squareIndex |= 2;
				if (gridValue [x + 1 + offy1] < isovalue)
					squareIndex |= 4;
				if (gridValue [x + offy1] < isovalue)
					squareIndex |= 8;
			} else {
				if (gridValue [x + offy] > isovalue)
					squareIndex |= 1;
				if (gridValue [x + 1 + offy] > isovalue)
					squareIndex |= 2;
				if (gridValue [x + 1 + offy1] > isovalue)
					squareIndex |= 4;
				if (gridValue [x + offy1] > isovalue)
					squareIndex |= 8;
			}	
			return squareIndex;
		}


		//--------------------------------------------
		// getEdgeVertex()
		//--------------------------------------------
		public EdgeVertex getEdgeVertex (int index)
		{
			return edgeVrt [index];
		}

	};
}

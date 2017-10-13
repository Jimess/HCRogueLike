using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
	public float leftX, leftY, rightX, rightY;
	public const int GROUND_X = 0;
	public const int GROUND_Y = 0;

	public float offsetX, offsetY;
	public float squareNum;
	public float offsetYTOP;
	public float offsetXTOP;
	public float topLineScale;

	public float spriteHeight;

	public float angle;

	public Square (float x1, float x2, float y2, float y1, float squareNum, float spriteHeight) {
		leftX = x1;
		leftY = y1;
		rightX = x2;
		rightY = y2;
		this.spriteHeight = spriteHeight;

		offsetX = (rightX - leftX) / 2;
		offsetY = 1f - (spriteHeight/2);
		//the lenght from the bottom line to the middle of the top line
		offsetYTOP = (leftY - offsetY + ((rightY - leftY) / 2));
		
		topLineScale = Vector2.Distance (new Vector2(x2, y2), new Vector2(x1, y1)); // the lenght of the top lines

		angle = Mathf.Acos (1f / topLineScale) * Mathf.Rad2Deg;
		if (leftY > rightY) {
			angle = -angle;
		}

		this.squareNum = squareNum;
	}

	public List<Vector2> GetPoints () {
		List<Vector2> pointList = new List<Vector2> {
			new Vector2 (leftX - offsetX - squareNum, leftY - offsetY),
			new Vector2 (rightX - offsetX - squareNum, rightY - offsetY),
			new Vector2 (rightX - offsetX - squareNum, GROUND_Y - offsetY),
			new Vector2 (leftX - offsetX - squareNum, GROUND_Y - offsetY)
		};
		return pointList;
	}
}

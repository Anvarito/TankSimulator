namespace ChobiAssets.PTM
{

	public class Gun_Camera_Input_00_Base_CS
	{

		protected Gun_Camera_CS gunCameraScript;


		public virtual void Prepare(Gun_Camera_CS gunCameraScript)
		{
			this.gunCameraScript = gunCameraScript;
		}


		public virtual void Get_Input()
		{
		}

		public virtual void DissableInput()
        {

        }

	}

}

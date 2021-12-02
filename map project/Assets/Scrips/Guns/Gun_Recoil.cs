using UnityEngine;

public class Gun_Recoil : MonoBehaviour
{
    //Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    //Settings
    [SerializeField] private float snappines;
    [SerializeField] private float returnSpeed;

   

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation,Vector3.zero, returnSpeed* Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappines * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire(float recoilX, float recoilY, float recoilZ){
        
            if(Input.GetKey(KeyCode.Mouse1)){
                // ADS 
                targetRotation += new Vector3(recoilX/3, Random.Range(-recoilY/3,recoilY/3),Random.Range(-recoilZ/3,recoilZ/3));
            }
            else{
                // hipFire
                targetRotation += new Vector3(recoilX, Random.Range(-recoilY,recoilY),Random.Range(-recoilZ,recoilZ));
            }
        
    }
}

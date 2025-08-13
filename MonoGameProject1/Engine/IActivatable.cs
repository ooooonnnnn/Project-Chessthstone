namespace MonoGameProject1;

public interface IActivatable
{
	public void SetActive(bool active);
	public bool isActive { get; }
}
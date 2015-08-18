public interface JDIHaveScriptHandles
{
    event MonoScriptEventHandler ScriptAwake;
    event MonoScriptEventHandler ScriptUpdate;
    event MonoScriptEventHandler ScriptDestroy;

    void Awake();
    void Update();
    void Destroy();
}
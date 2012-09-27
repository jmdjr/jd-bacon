public interface JDIHaveScriptHandles
{
    event MonoScriptEventHandler ScriptAwake;
    event MonoScriptEventHandler ScriptUpdate;
    event MonoScriptEventHandler ScriptDestroy;
    event MonoScriptEventHandler ScriptCollisionEnter;
    event MonoScriptEventHandler ScriptCollisionStay;
    event MonoScriptEventHandler ScriptCollisionExit;
}
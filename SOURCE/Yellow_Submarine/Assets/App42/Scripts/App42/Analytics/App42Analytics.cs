using UnityEngine;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.app42Event;
using AssemblyCSharp;

public sealed class App42Analytics : MonoBehaviour {

//	[SerializeField]
//	private bool isDebug;


	private static EventService eventService;

	/// <summary>
	/// Inicializa o analytics.
	/// </summary>
	public static void Initialize(){
		Initialize(null);
	}

	/// <summary>
	/// Inicializa o analytics com um usuario.
	/// </summary>
	/// <param name="p_loggedUser">P_logged user.</param>
	public static void Initialize(string p_loggedUser = null){

		App42API.Initialize(App42Manager.Instance.API_KEY, App42Manager.Instance.SECRET_KEY);  
		App42API.EnableEventService(true);  //FIXME
		if (!string.IsNullOrEmpty( p_loggedUser))
			App42API.SetLoggedInUser(p_loggedUser) ; 

		eventService = App42API.BuildEventService(); 

//		if (isDebug)
//			App42Log.SetDebug(true);        //Prints output in your editor console  
	}

	#region Track Event
	/// <summary>
	/// Registra o acontecimento de um evento na aplicaçao.
	/// </summary>
	/// <param name="p_eventname">O nome do evento, ja cadastrado no dashboard .</param>
	/// <param name="p_properties">Propriedade dos eventos.</param>
	/// <param name="p_callBack">Funçao de retorno da solicitaçao.</param>
	/// <example>
	/// String eventName = "Purchase";  
	/// Dictionary<String,object> properties = new Dictionary<string, object> ();  
	/// properties.Add ("Name", "Nick");  
	/// properties.Add ("Revenue", 5000);  
	/// eventService.TrackEvent(eventName, properties, new UnityCallBack());   
	/// </example>
	public static void TrackEvent(string p_eventname, Dictionary<string,object> p_properties, App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException){
		App42Response response = new App42Response(pSuccess, pException);
		eventService.TrackEvent(p_eventname, p_properties, response);   
	}
	#endregion

	#region  SessionT racking
	/// <summary>
	/// Uma sessao eh definida pelo tempo total decorrido entre o momento que a aplicaçao foi aberta e fechada.
	/// </summary>
	public static void EnableSessionTracking(bool state){
		App42API.EnableAppStateEventTracking(state);  
	}	
	#endregion

	#region Activity
	/// <summary>
	/// Registra o tempo total gasto por um usuario	para completar um tarefa especifica.
	/// </summary>
	/// <param name="p_activityName">Nome da atividade.</param>
	/// <param name="p_properties">Propriedade da atividade.</param>
	/// <param name="p_callBack">Funçao de retorno da solicitaçao.</param>
	/// <example>
	/// String activityName = "Level1";  
	/// Dictionary<String,object> properties = new Dictionary<string, object> ();  
	/// properties.Add ("Name", "Level1");  
	/// properties.Add ("State", "Started");  
	/// properties.Add ("Score", 0);  
	/// properties.Add ("Difficulty", "Easy");  
	/// properties.Add ("IsCompletedSecretMission", false);  
	/// eventService.StartActivity(activityName, properties, new UnityCallBack());  
	/// </example>
	public static void StartTrackingUserActivity(string p_activityName, Dictionary<string,object> p_properties, App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException){
		App42Response response = new App42Response(pSuccess, pException);
		eventService.StartActivity(p_activityName, p_properties, response); 
	}

	
	/// <summary>
	/// Finaliza o rastreamento de uma atividade registrada.
	/// </summary>
	/// <param name="p_activityName">P_activity name.</param>
	/// <param name="p_properties">P_properties.</param>
	/// <param name="p_callBack">P_call back.</param>
	/// <example>
	/// String activityName = "Level1";  
	/// Dictionary<String,object> properties = new Dictionary<string, object> ();  
	/// properties.Add ("Name", "Level1");  
	/// properties.Add ("State", "Ended");  
	/// properties.Add ("Score", 40000);  
	/// properties.Add ("Difficulty", "Easy");  
	/// properties.Add ("IsCompletedSecretMission", true);  
	/// eventService.EndActivity(activityName, properties, new UnityCallBack());  
	/// </example>
	public static void EndTrackingUserActivity(string p_activityName, Dictionary<string,object> p_properties, App42Response.OnSuccessDelegate pSuccess, App42Response.OnExceptionDelegate pException){
		App42Response response = new App42Response(pSuccess, pException);
		eventService.EndActivity(p_activityName, p_properties, response); 
	}
	#endregion


	#region Crash Report
	/// <summary>
	/// Registra erros ocorridos na aplicaçao.
	/// </summary>
	public static void EnableCrashReport(){
		App42API.EnableCrashEventHandler(true); 
	}
	#endregion

}

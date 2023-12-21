using Steamworks;

using System;
using System.Text;
using UnityEngine;

namespace Superfine.Unity
{
    public static class SuperfineSDKSteamworks
    {
        public static class EventNames
        {
            //--ISteamAppList--
            public const string APP_INSTALLED = "steam_app_installed";
            public const string APP_UNINSTALLED = "steam_app_uninstalled";

            //--ISteamApps--
            public const string DLC_INSTALLED = "steam_dlc_installed";
            public const string NEW_URL_LAUNCH_PARAMETERS = "steam_new_url_launch_parameters";
            public const string APP_PROOF_OF_PURCHASE_KEY_RESPONSE = "steam_app_proof_of_purchase_key_response";
            public const string FILE_DETAILS_RESULT = "steam_file_details_result";
            public const string TIMED_TRIAL_STATUS = "steam_timed_trial_status";

            //--ISteamFriends--
            public const string PERSONA_STATE_CHANGE = "steam_persona_state_change";
            public const string GAME_OVERLAY_ACTIVATED = "steam_game_overlay_activated";
            public const string GAME_SERVER_CHANGE_REQUESTED = "steam_game_server_change_requested";
            public const string GAME_LOBBY_JOIN_REQUESTED = "steam_game_lobby_join_requested";
            public const string AVATAR_IMAGE_LOADED = "steam_avatar_image_loaded";
            public const string CLAN_OFFICER_LIST_RESPONSE = "steam_clan_officer_list_response";
            public const string FRIEND_RICH_PRESENCE_UPDATE = "steam_friend_rich_presence_update";
            public const string GAME_RICH_PRESENCE_JOIN_REQUESTED = "steam_game_rich_presence_join_requested";
            public const string GAME_CONNECTED_CLAN_CHAT_MSG = "steam_game_connected_clan_chat_msg";
            public const string GAME_CONNECTED_CHAT_JOIN = "steam_game_connected_chat_join";
            public const string GAME_CONNECTED_CHAT_LEAVE = "steam_game_connected_chat_leave";
            public const string DOWNLOAD_CLAN_ACTIVITY_COUNTS_RESULT = "steam_download_clan_activity_counts_result";
            public const string JOIN_CLAN_CHAT_ROOM_COMPLETION_RESULT = "steam_join_clan_chat_room_completion_result";
            public const string GAME_CONNECTED_FRIEND_CHAT_MSG = "steam_game_connected_friend_chat_msg";
            public const string FRIENDS_GET_FOLLOWER_COUNT = "steam_friends_get_follower_count";
            public const string FRIENDS_IS_FOLLOWING = "steam_friends_is_following";
            public const string FRIENDS_ENUMERATE_FOLLOWING_LIST = "steam_friends_enumerate_following_list";
            public const string SET_PERSONA_NAME_RESPONSE = "steam_set_persona_name_response";
            public const string UNREAD_CHAT_MESSAGES_CHANGED = "steam_unread_chat_messages_changed";
            public const string OVERLAY_BROWSER_PROTOCOL_NAVIGATION = "steam_overlay_browser_protocol_navigation";
            public const string EQUIPPED_PROFILE_ITEMS_CHANGED = "steam_equipped_profile_items_changed";
            public const string EQUIPPED_PROFILE_ITEMS = "steam_equipped_profile_items";

            //--IStreamGameCoordinator--
            public const string GC_MESSAGE_AVAILABLE = "steam_gc_message_available";
            public const string GC_MESSAGE_FAILED = "steam_gc_message_failed";

            //--IStreamGameServer--
            public const string GS_CLIENT_APPROVE = "steam_gs_client_approve";
            public const string GS_CLIENT_DENY = "steam_gs_client_deny";
            public const string GS_CLIENT_KICK = "steam_gs_client_kick";
            public const string GS_CLIENT_ACHIEVEMENT_STATUS = "steam_gs_client_achievement_status";
            public const string GS_POLICY_RESPONSE = "steam_gs_policy_response";
            public const string GS_GAMEPLAY_STATS = "steam_gs_gameplay_stats";
            public const string GS_CLIENT_GROUP_STATUS = "steam_gs_client_group_status";
            public const string GS_REPUTATION = "steam_gs_reputation";
            public const string ASSOCIATE_WITH_CLAN_RESULT = "steam_associate_with_clan_result";
            public const string COMPUTE_NEW_PLAYER_COMPATIBILITY_RESULT = "steam_compute_new_player_compatibility_result";

            //--IStreamGameServerStats--
            public const string GS_STATS_RECEIVED = "steam_gs_stats_received";
            public const string GS_STATS_STORED = "steam_gs_stats_stored";
            public const string GS_STATS_UNLOADED = "steam_gs_stats_unloaded";

            //--ISteamHTMLSurface--
            public const string HTML_BROWSER_READY = "steam_html_browser_ready";
            public const string HTML_NEEDS_PAINT = "steam_html_needs_paint";
            public const string HTML_START_REQUEST = "steam_html_start_request";
            public const string HTML_CLOSE_BROWSER = "steam_html_close_browser";
            public const string HTML_URL_CHANGED = "steam_html_url_changed";
            public const string HTML_FINISHED_REQUEST = "steam_html_finished_request";
            public const string HTML_OPEN_LINK_IN_NEW_TAB = "steam_html_open_link_in_new_tab";
            public const string HTML_CHANGED_TITLE = "steam_html_changed_title";
            public const string HTML_SEARCH_RESULTS = "steam_html_search_results";
            public const string HTML_CAN_GO_BACK_AND_FORWARD = "steam_html_can_go_back_and_forward";
            public const string HTML_HORIZONTAL_SCROLL = "steam_html_horizontalScroll";
            public const string HTML_VERTICAL_SCROLL = "steam_html_verticalScroll";
            public const string HTML_LINK_AT_POSITION = "steam_html_link_at_position";
            public const string HTML_JS_ALERT = "steam_html_js_alert";
            public const string HTML_JS_CONFIRM = "steam_html_js_confirm";
            public const string HTML_FILE_OPEN_DIALOG = "steam_html_file_open_dialog";
            public const string HTML_NEW_WINDOW = "steam_html_new_window";
            public const string HTML_SET_CURSOR = "steam_html_set_cursor";
            public const string HTML_STATUS_TEXT = "steam_html_status_text";
            public const string HTML_SHOW_TOOL_TIP = "steam_html_show_tool_tip";
            public const string HTML_UPDATE_TOOL_TIP = "steam_html_update_tool_tip";
            public const string HTML_HIDE_TOOL_TIP = "steam_html_hide_tool_tip";
            public const string HTML_BROWSER_RESTARTED = "steam_html_browser_restarted";

            //--ISteamHTTP--
            public const string HTTP_REQUEST_COMPLETED = "steam_http_request_completed";
            public const string HTTP_REQUEST_HEADERS_RECEIVED = "steam_http_request_headers_received";
            public const string HTTP_REQUEST_DATA_RECEIVED = "steam_http_request_data_received";

            //--ISteamInput--
            public const string INPUT_DEVICE_CONNECTED = "steam_input_device_connected";
            public const string INPUT_DEVICE_DISCONNECTED = "steam_input_device_disconnected";
            public const string INPUT_CONFIGURATION_LOADED = "steam_input_configuration_loaded";
            public const string INPUT_GAMEPAD_SLOT_CHANGE = "steam_input_gamepad_slot_change";

            //--ISteamInventory--
            public const string INVENTORY_RESULT_READY = "steam_inventory_result_ready";
            public const string INVENTORY_FULL_UPDATE = "steam_inventory_full_update";
            public const string INVENTORY_DEFINITION_UPDATE = "steam_inventory_definition_update";
            public const string INVENTORY_ELIGIBLE_PROMO_ITEM_DEF_IDS = "steam_inventory_eligible_promo_item_def_ids";
            public const string INVENTORY_START_PURCHASE_RESULT = "steam_inventory_start_purchase_result";
            public const string INVENTORY_REQUEST_PRICES_RESULT = "steam_inventory_request_prices_result";

            //--ISteamMatchmaking--
            public const string FAVORITES_LIST_CHANGED = "steam_favorites_list_changed";
            public const string LOBBY_INVITE = "steam_lobby_invite";
            public const string LOBBY_ENTER = "steam_lobby_enter";
            public const string LOBBY_DATA_UPDATE = "steam_lobby_data_update";
            public const string LOBBY_CHAT_UPDATE = "steam_lobby_chat_update";
            public const string LOBBY_CHAT_MSG = "steam_lobby_chat_msg";
            public const string LOBBY_GAME_CREATED = "steam_lobby_game_created";
            public const string LOBBY_MATCH_LIST = "steam_lobby_match_list";
            public const string LOBBY_KICKED = "steam_lobby_kicked";
            public const string LOBBY_CREATED = "steam_lobby_created";
            public const string PSN_GAME_BOOT_INVITE_RESULT = "steam_psn_game_boot_invite_result";
            public const string FAVORITES_LIST_ACCOUNTS_UPDATED = "steam_favorites_list_accounts_updated";
            public const string SEARCH_FOR_GAME_PROGRESS_CALLBACK = "steam_search_for_game_progress_callback";
            public const string SEARCH_FOR_GAME_RESULT_CALLBACK = "steam_search_for_game_result_callback";
            public const string REQUEST_PLAYERS_FOR_GAME_PROGRESS_CALLBACK = "steam_request_players_for_game_progress_callback";
            public const string REQUEST_PLAYERS_FOR_GAME_RESULT_CALLBACK = "steam_request_players_for_game_result_callback";
            public const string REQUEST_PLAYERS_FOR_GAME_FINAL_RESULT_CALLBACK = "steam_request_players_for_game_final_result_callback";
            public const string SUBMIT_PLAYER_RESULT_CALLBACK = "steam_submit_player_result_callback";
            public const string END_GAME_RESULT_CALLBACK = "steam_end_game_result_callback";
            public const string JOIN_PARTY_CALLBACK = "steam_join_party_callback";
            public const string CREATE_BEACON_CALLBACK = "steam_create_beacon_callback";
            public const string RESERVATION_NOTIFICATION_CALLBACK = "steam_reservation_notification_callback";
            public const string CHANGE_NUM_OPEN_SLOTS_CALLBACK = "steam_change_num_open_slots_callback";
            public const string AVAILABLE_BEACON_LOCATIONS_UPDATED = "steam_available_beacon_locations_updated";
            public const string ACTIVE_BEACONS_UPDATED = "steam_active_beacons_updated";

            //--ISteamMusic--
            public const string PLAYBACK_STATUS_HAS_CHANGED = "steam_playback_status_has_changed";
            public const string VOLUME_HAS_CHANGED = "steam_volume_has_changed";

            //--ISteamMusicRemote--
            public const string MUSIC_PLAYER_REMOTE_WILL_ACTIVATE = "steam_music_player_remote_will_activate";
            public const string MUSIC_PLAYER_REMOTE_WILL_DEACTIVATE = "steam_music_player_remote_will_deactivate";
            public const string MUSIC_PLAYER_REMOTE_TO_FRONT = "steam_music_player_remote_to_front";
            public const string MUSIC_PLAYER_WILL_QUIT = "steam_music_player_will_quit";
            public const string MUSIC_PLAYER_WANTS_PLAY = "steam_music_player_wants_play";
            public const string MUSIC_PLAYER_WANTS_PAUSE = "steam_music_player_wants_pause";
            public const string MUSIC_PLAYER_WANTS_PLAY_PREVIOUS = "steam_music_player_wants_play_previous";
            public const string MUSIC_PLAYER_WANTS_PLAY_NEXT = "steam_music_player_wants_play_next";
            public const string MUSIC_PLAYER_WANTS_SHUFFLED = "steam_music_player_wants_shuffled";
            public const string MUSIC_PLAYER_WANTS_LOOPED = "steam_music_player_wants_looped";
            public const string MUSIC_PLAYER_WANTS_VOLUME = "steam_music_player_wants_volume";
            public const string MUSIC_PLAYER_SELECTS_QUEUE_ENTRY = "steam_music_player_selects_queue_entry";
            public const string MUSIC_PLAYER_SELECTS_PLAYLIST_ENTRY = "steam_music_player_selects_playlist_entry";
            public const string MUSIC_PLAYER_WANTS_PLAYING_REPEAT_STATUS = "steam_music_player_wants_playing_repeat_status";

            //--ISteamNetworking--
            public const string P2P_SESSION_REQUEST = "steam_p2p_session_request";
            public const string P2P_SESSION_CONNECT_FAIL = "steam_p2p_session_connect_fail";
            public const string SOCKET_STATUS_CALLBACK = "steam_socket_status_callback";

            //--ISteamNetworkingMessages--
            public const string NETWORKING_MESSAGES_SESSION_REQUEST = "steam_networking_messages_session_request";
            public const string NETWORKING_MESSAGES_SESSION_FAILED = "steam_networking_messages_session_failed";

            //--ISteamNetworkingSockets--
            public const string NET_CONNECTION_STATUS_CHANGED_CALLBACK = "steam_net_connection_status_changed_callback";
            public const string NET_AUTHENTICATION_STATUS = "steam_net_authentication_status";
            public const string NETWORKING_FAKE_IP_RESULT = "steam_networking_fake_ip_result";

            //--ISteamNetworkingUtils--
            public const string RELAY_NETWORK_STATUS = "steam_relay_network_status";

            //--ISteamParentalSettings--
            public const string PARENTAL_SETTINGS_CHANGED = "steam_parental_settings_changed";

            //--ISteamRemotePlay--
            public const string REMOTE_PLAY_SESSION_CONNECTED = "steam_remote_play_session_connected";
            public const string REMOTE_PLAY_SESSION_DISCONNECTED = "steam_remote_play_session_disconnected";
            public const string REMOTE_PLAY_TOGETHER_GUEST_INVITE = "steam_remote_play_together_guest_invite";

            //--ISteamRemoteStorage--
            public const string REMOTE_STORAGE_FILE_SHARE_RESULT = "steam_remote_storage_file_share_result";
            public const string REMOTE_STORAGE_PUBLISH_FILE_RESULT = "steam_remote_storage_publish_file_result";
            public const string REMOTE_STORAGE_DELETE_PUBLISHED_FILE_RESULT = "steam_remote_storage_delete_published_file_result";
            public const string REMOTE_STORAGE_ENUMERATE_USER_PUBLISHED_FILES_RESULT = "steam_remote_storage_enumerate_user_published_files_result";
            public const string REMOTE_STORAGE_SUBSCRIBE_PUBLISHED_FILE_RESULT = "steam_remote_storage_subscribe_published_file_result";
            public const string REMOTE_STORAGE_ENUMERATE_USER_SUBSCRIBED_FILES_RESULT = "steam_remote_storage_enumerate_user_subscribed_files_result";
            public const string REMOTE_STORAGE_UNSUBSCRIBE_PUBLISHED_FILE_RESULT = "steam_remote_storage_unsubscribe_published_file_result";
            public const string REMOTE_STORAGE_UPDATE_PUBLISHED_FILE_RESULT = "steam_remote_storage_update_published_file_result";
            public const string REMOTE_STORAGE_DOWNLOAD_UGC_RESULT = "steam_remote_storage_download_ugc_result";
            public const string REMOTE_STORAGE_GET_PUBLISHED_FILE_DETAILS_RESULT = "steam_remote_storage_get_published_file_details_result";
            public const string REMOTE_STORAGE_ENUMERATE_WORKSHOP_FILES_RESULT = "steam_remote_storage_enumerate_workshop_files_result";
            public const string REMOTE_STORAGE_GET_PUBLISHED_ITEM_VOTE_DETAILS_RESULT = "steam_remote_storage_get_published_item_vote_details_result";
            public const string REMOTE_STORAGE_PUBLISHED_FILE_SUBSCRIBED = "steam_remote_storage_published_file_subscribed";
            public const string REMOTE_STORAGE_PUBLISHED_FILE_UNSUBSCRIBED = "steam_remote_storage_published_file_unsubscribed";
            public const string REMOTE_STORAGE_PUBLISHED_FILE_DELETED = "steam_remote_storage_published_file_deleted";
            public const string REMOTE_STORAGE_UPDATE_USER_PUBLISHED_ITEM_VOTE_RESULT = "steam_remote_storage_update_user_published_item_vote_result";
            public const string REMOTE_STORAGE_USER_VOTE_DETAILS = "steam_remote_storage_user_vote_details";
            public const string REMOTE_STORAGE_ENUMERATE_USER_SHARED_WORKSHOP_FILES_RESULT = "steam_remote_storage_enumerate_user_shared_workshop_files_result";
            public const string REMOTE_STORAGE_SET_USER_PUBLISHED_FILE_ACTION_RESULT = "steam_remote_storage_set_user_published_file_action_result";
            public const string REMOTE_STORAGE_ENUMERATE_PUBLISHED_FILES_BY_USER_ACTION_RESULT = "steam_remote_storage_enumerate_published_files_by_user_action_result";
            public const string REMOTE_STORAGE_PUBLISH_FILE_PROGRESS = "steam_remote_storage_publish_file_progress";
            public const string REMOTE_STORAGE_PUBLISHED_FILE_UPDATED = "steam_remote_storage_published_file_updated";
            public const string REMOTE_STORAGE_FILE_WRITE_ASYNC_COMPLETE = "steam_remote_storage_file_write_async_complete";
            public const string REMOTE_STORAGE_FILE_READ_ASYNC_COMPLETE = "steam_remote_storage_file_read_async_complete";
            public const string REMOTE_STORAGE_LOCAL_FILE_CHANGE = "steam_remote_storage_local_file_change";

            //--ISteamScreenshots--
            public const string SCREENSHOT_READY = "steam_screenshot_ready";
            public const string SCREENSHOT_REQUESTED = "steam_screenshot_requested";

            //--ISteamUGC--
            public const string UGC_QUERY_COMPLETED = "steam_ugc_query_completed";
            public const string UGC_REQUEST_UGC_DETAILS_RESULT = "steam_ugc_request_ugc_ugc_details_result";
            public const string CREATE_ITEM_RESULT = "steam_create_item_result";
            public const string SUBMIT_ITEM_UPDATE_RESULT = "steam_submit_item_update_result";
            public const string ITEM_INSTALLED = "steam_item_installed";
            public const string DOWNLOAD_ITEM_RESULT = "steam_download_item_result";
            public const string USER_FAVORITE_ITEMS_LIST_CHANGED = "steam_user_favorite_items_list_changed";
            public const string SET_USER_ITEM_VOTE_RESULT = "steam_set_user_item_vote_result";
            public const string GET_USER_ITEM_VOTE_RESULT = "steam_get_user_item_vote_result";
            public const string START_PLAYTIME_TRACKING_RESULT = "steam_start_playtime_tracking_result";
            public const string STOP_PLAYTIME_TRACKING_RESULT = "steam_stop_playtime_tracking_result";
            public const string ADD_UGC_DEPENDENCY_RESULT = "steam_add_ugc_dependency_result";
            public const string REMOVE_UGC_DEPENDENCY_RESULT = "steam_remove_ugc_dependency_result";
            public const string ADD_APP_DEPENDENCY_RESULT = "steam_add_app_dependency_result";
            public const string REMOVE_APP_DEPENDENCY_RESULT = "steam_remove_app_dependency_result";
            public const string GET_APP_DEPENDENCIES_RESULT = "steam_get_app_dependencies_result";
            public const string DELETE_ITEM_RESULT = "steam_delete_item_result";
            public const string USER_SUBSCRIBED_ITEMS_LIST_CHANGED = "steam_user_subscribed_items_list_changed";
            public const string WORKSHOP_EULA_STATUS = "steam_workshop_eula_status";

            //--ISteamUser--
            public const string STEAM_SERVERS_CONNECTED = "steam_steam_servers_connected";
            public const string STEAM_SERVER_CONNECT_FAILURE = "steam_steam_server_connect_failure";
            public const string STEAM_SERVERS_DISCONNECTED = "steam_steam_servers_disconnected";
            public const string CLIENT_GAME_SERVER_DENY = "steam_client_game_server_deny";
            public const string IPC_FAILURE = "steam_ipc_failure";
            public const string LICENSES_UPDATED = "steam_licenses_updated";
            public const string VALIDATE_AUTH_TICKET_RESPONSE = "steam_validate_auth_ticket_response";
            public const string MICRO_TXN_AUTHORIZATION_RESPONSE = "steam_micro_txn_authorization_response";
            public const string ENCRYPTED_APP_TICKET_RESPONSE = "steam_encrypted_app_ticket_response";
            public const string GET_AUTH_SESSION_TICKET_RESPONSE = "steam_get_auth_session_ticket_response";
            public const string GAME_WEB_CALLBACK = "steam_game_web_callback";
            public const string STORE_AUTH_URL_RESPONSE = "steam_store_auth_url_response";
            public const string MARKET_ELIGIBILITY_RESPONSE = "steam_market_eligibility_response";
            public const string DURATION_CONTROL = "steam_duration_control";
            public const string GET_TICKET_FOR_WEB_API_RESPONSE = "steam_get_ticket_for_web_api_response";

            //--ISteamUserStats--
            public const string USER_STATS_RECEIVED = "steam_user_stats_received";
            public const string USER_STATS_STORED = "steam_user_stats_stored";
            public const string USER_ACHIEVEMENT_STORED = "steam_user_achievement_stored";
            public const string LEADERBOARD_FIND_RESULT = "steam_leaderboard_find_result";
            public const string LEADERBOARD_SCORES_DOWNLOADED = "steam_leaderboard_scores_downloaded";
            public const string LEADERBOARD_SCORE_UPLOADED = "steam_leaderboard_score_uploaded";
            public const string NUMBER_OF_CURRENT_PLAYERS = "steam_number_of_current_players";
            public const string USER_STATS_UNLOADED = "steam_user_stats_unloaded";
            public const string USER_ACHIEVEMENT_ICON_FETCHED = "steam_user_achievement_icon_fetched";
            public const string GLOBAL_ACHIEVEMENT_PERCENTAGES_READY = "steam_global_achievement_percentages_ready";
            public const string LEADERBOARD_UGC_SET = "steam_leaderboard_ugc_set";
            public const string PS3_TROPHIES_INSTALLED = "steam_ps3_trophies_installed";
            public const string GLOBAL_STATS_RECEIVED = "steam_global_stats_received";

            //--ISteamUtils--
            public const string IP_COUNTRY = "steam_ip_country";
            public const string LOW_BATTERY_POWER = "steam_low_battery_power";
            public const string STEAM_API_CALL_COMPLETED = "steam_steam_api_call_completed";
            public const string STEAM_SHUTDOWN = "steam_steam_shutdown";
            public const string CHECK_FILE_SIGNATURE = "steam_check_file_signature";
            public const string GAMEPAD_TEXT_INPUT_DISMISSED = "steam_gamepad_text_input_dismissed";
            public const string APP_RESUMING_FROM_SUSPEND = "steam_app_resuming_from_suspend";
            public const string FLOATING_GAMEPAD_TEXT_INPUT_DISMISSED = "steam_floating_gamepad_text_input_dismissed";
            public const string FILTER_TEXT_DICTIONARY_CHANGED = "steam_filter_text_dictionary_changed";

            //--ISteamVideo--
            public const string GET_VIDEO_URL_RESULT = "steam_get_video_url_result";
            public const string GET_OPF_SETTINGS_RESULT = "steam_get_opf_settings_result";
        }

#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if !UNITY_EDITOR && UNITY_STANDALONE_OSX
            SuperfineSDK.OnPostInit += (manager) =>
            {
                manager.SetSteamDRMCheck(SteamDRMCheck);
            };
#endif
        }
#endif

        private static int SteamDRMCheck(uint appId)
        {
            return SteamAPI.RestartAppIfNecessary(new AppId_t(appId)) ? 1 : 0;
        }

        private static void PutString(SimpleJSON.JSONObject obj, string key, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            obj.Add(key, value);
        }

        private static void PutInt(SimpleJSON.JSONObject obj, string key, int value)
        {
            obj.Add(key, value);
        }

        private static void PutUint(SimpleJSON.JSONObject obj, string key, uint value)
        {
            obj.Add(key, value);
        }

        private static void PutLong(SimpleJSON.JSONObject obj, string key, long value)
        {
            obj.Add(key, value);
        }

        private static void PutUlong(SimpleJSON.JSONObject obj, string key, ulong value)
        {
            obj.Add(key, value);
        }

        private static void PutByte(SimpleJSON.JSONObject obj, string key, byte value)
        {
            obj.Add(key, value);
        }

        private static void PutBool(SimpleJSON.JSONObject obj, string key, bool value)
        {
            obj.Add(key, value);
        }

        private static void PutFloat(SimpleJSON.JSONObject obj, string key, float value)
        {
            obj.Add(key, value);
        }

        private static void PutDouble(SimpleJSON.JSONObject obj, string key, double value)
        {
            obj.Add(key, value);
        }

        private static void PutByteArray(SimpleJSON.JSONObject obj, string key, byte[] value)
        {
            obj.Add(key, BitConverter.ToString(value).Replace("-", string.Empty));
        }

        private static void PutByteArray(SimpleJSON.JSONObject obj, string key, byte[] value, int count)
        {
            obj.Add(key, BitConverter.ToString(value, 0, count).Replace("-", string.Empty));
        }

        private static void PutJSONObject(SimpleJSON.JSONObject obj, string key, SimpleJSON.JSONObject value)
        {
            obj.Add(key, value);
        }

        private static void PutJSONArray(SimpleJSON.JSONObject obj, string key, SimpleJSON.JSONArray value)
        {
            obj.Add(key, value);
        }

        private static string GetIPv4String(uint ip)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(ip >> 24);
            stringBuilder.Append(':');
            stringBuilder.Append((ip >> 16) & 255);
            stringBuilder.Append(':');
            stringBuilder.Append((ip >> 8) & 255);
            stringBuilder.Append(':');
            stringBuilder.Append(ip & 255);
            
            return stringBuilder.ToString();
        }

        private static string GetIPv6String(byte[] ipv6)
        {
            StringBuilder stringBuilder = new StringBuilder();

            int len = ipv6.Length;

            for (var i = 0; i < len; i += 2)
            {
                if (i > 0) stringBuilder.Append(':');

                int segment = ipv6[i] << 8 | ipv6[i + 1];
                stringBuilder.AppendFormat("{0:X2}", segment);
            }

            return stringBuilder.ToString();
        }

        private static void PutSteamNetworkingIPAddr(SimpleJSON.JSONObject obj, ref SteamNetworkingIPAddr addr)
        {
            if (addr.IsIPv4())
            {
                PutString(obj, "ip", GetIPv4String(addr.GetIPv4()));
            }
            else
            {
                PutString(obj, "ipv6", GetIPv6String(addr.m_ipv6));
            }

            PutUint(obj, "port", addr.m_port);
        }

        private static void PutSteamNetworkingIPAddr(SimpleJSON.JSONObject obj, string key, ref SteamNetworkingIPAddr addr)
        {
            SimpleJSON.JSONObject data = new SimpleJSON.JSONObject();
            PutSteamNetworkingIPAddr(data, ref addr);

            PutJSONObject(obj, key, data);
        }

        private static SimpleJSON.JSONObject GetJSONObjectFromSteamNetworkingIdentity(ref SteamNetworkingIdentity steamNetworkingIdentity)
        {
            SimpleJSON.JSONObject data = new SimpleJSON.JSONObject();

            ESteamNetworkingIdentityType type = steamNetworkingIdentity.m_eType;
            PutInt(data, "type", (int)type);

            switch (type)
            {
                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_SteamID:
                    PutUlong(data, "steamId", steamNetworkingIdentity.GetSteamID64());
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_XboxPairwiseID:
                    PutString(data, "xboxPairwiseId", steamNetworkingIdentity.GetXboxPairwiseID());
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_SonyPSN:
                    PutUlong(data, "psnId", steamNetworkingIdentity.GetPSNID());
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_GoogleStadia:
                    PutUlong(data, "stadiaId", steamNetworkingIdentity.GetStadiaID());
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_IPAddress:
                    {
                        try
                        {
                            SteamNetworkingIPAddr addr = steamNetworkingIdentity.GetIPAddr();
                            PutSteamNetworkingIPAddr(data, ref addr);
                        }
                        catch (Exception)
                        {
                            uint ipv4 = steamNetworkingIdentity.GetIPv4();
                            if (ipv4 > 0) PutString(data, "ip", GetIPv4String(ipv4));
                        }

                        ESteamNetworkingFakeIPType fakeIPType = steamNetworkingIdentity.GetFakeIPType();
                        if (fakeIPType > ESteamNetworkingFakeIPType.k_ESteamNetworkingFakeIPType_NotFake)
                        {
                            PutInt(data, "fakeIpType", (int)fakeIPType);
                        }
                    }
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_GenericString:
                    PutString(data, "genericString", steamNetworkingIdentity.GetGenericString());
                    break;

                case ESteamNetworkingIdentityType.k_ESteamNetworkingIdentityType_GenericBytes:
                    {
                        try
                        {
                            byte[] arr = steamNetworkingIdentity.GetGenericBytes(out int len);
                            PutByteArray(data, "genericBytes", arr, len);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    break;

                default:
                    break;
            }

            return data;
        }

        private static SimpleJSON.JSONObject GetJSONObjectFromSteamNetConnectionInfo(ref SteamNetConnectionInfo_t steamNetConnectionInfo)
        {
            SimpleJSON.JSONObject data = new SimpleJSON.JSONObject();

            PutJSONObject(data, "remoteIdentity", GetJSONObjectFromSteamNetworkingIdentity(ref steamNetConnectionInfo.m_identityRemote));
            PutLong(data, "userData", steamNetConnectionInfo.m_nUserData);
            PutUint(data, "listenSocket", steamNetConnectionInfo.m_hListenSocket.m_HSteamListenSocket);
            PutSteamNetworkingIPAddr(data, "remoteAddr", ref steamNetConnectionInfo.m_addrRemote);
            PutUint(data, "remotePopId", steamNetConnectionInfo.m_idPOPRemote.m_SteamNetworkingPOPID);
            PutUint(data, "relayPopId", steamNetConnectionInfo.m_idPOPRelay.m_SteamNetworkingPOPID);
            PutInt(data, "state", (int)steamNetConnectionInfo.m_eState);
            PutInt(data, "endReason", steamNetConnectionInfo.m_eEndReason);
            PutString(data, "endDebug", steamNetConnectionInfo.m_szEndDebug);
            PutString(data, "connectionDescription", steamNetConnectionInfo.m_szConnectionDescription);
            PutInt(data, "flags", steamNetConnectionInfo.m_nFlags);

            return data;
        }

        private static SimpleJSON.JSONObject GetJSONObjectFromSteamUGCDetails(ref SteamUGCDetails_t steamUGCDetails)
        {
            SimpleJSON.JSONObject data = new SimpleJSON.JSONObject();

            PutUlong(data, "publishedFileId", steamUGCDetails.m_nPublishedFileId.m_PublishedFileId);
            PutInt(data, "result", (int)steamUGCDetails.m_eResult);
            PutInt(data, "fileType", (int)steamUGCDetails.m_eFileType);
            PutUint(data, "creatorAppId", steamUGCDetails.m_nCreatorAppID.m_AppId);
            PutUint(data, "consumerAppID", steamUGCDetails.m_nConsumerAppID.m_AppId);
            PutString(data, "title", steamUGCDetails.m_rgchTitle);
            PutString(data, "description", steamUGCDetails.m_rgchDescription);
            PutUlong(data, "ownerSteamId", steamUGCDetails.m_ulSteamIDOwner);
            PutUint(data, "createdTime", steamUGCDetails.m_rtimeCreated);
            PutUint(data, "updatedTime", steamUGCDetails.m_rtimeUpdated);
            PutUint(data, "addedToUserListTime", steamUGCDetails.m_rtimeAddedToUserList);
            PutInt(data, "visibility", (int)steamUGCDetails.m_eVisibility);
            PutBool(data, "banned", steamUGCDetails.m_bBanned);
            PutBool(data, "acceptedForUse", steamUGCDetails.m_bAcceptedForUse);
            PutBool(data, "tagsTruncated", steamUGCDetails.m_bTagsTruncated);
            PutString(data, "tags", steamUGCDetails.m_rgchTags);
            PutUlong(data, "file", steamUGCDetails.m_hFile.m_UGCHandle);
            PutUlong(data, "previewFile", steamUGCDetails.m_hPreviewFile.m_UGCHandle);
            PutString(data, "filename", steamUGCDetails.m_pchFileName);
            PutInt(data, "fileSize", steamUGCDetails.m_nFileSize);
            PutInt(data, "previewFileSize", steamUGCDetails.m_nPreviewFileSize);
            PutString(data, "url", steamUGCDetails.m_rgchURL);
            PutUint(data, "votesUp", steamUGCDetails.m_unVotesUp);
            PutUint(data, "votesDown", steamUGCDetails.m_unVotesDown);
            PutFloat(data, "score", steamUGCDetails.m_flScore);
            PutUint(data, "numChildren", steamUGCDetails.m_unNumChildren);

            return data;
        }

        public static void LogAppInstalled(ref SteamAppInstalled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_nAppID.m_AppId);
            PutInt(logData, "installFolderId", data.m_iInstallFolderIndex);

            SuperfineSDK.Log(EventNames.APP_INSTALLED, logData);
        }

        public static void LogAppUninstalled(ref SteamAppUninstalled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_nAppID.m_AppId);
            PutInt(logData, "installFolderId", data.m_iInstallFolderIndex);

            SuperfineSDK.Log(EventNames.APP_UNINSTALLED, logData);
        }

        public static void LogDlcInstalled(ref DlcInstalled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.DLC_INSTALLED, logData);
        }

        public static void LogNewUrlLaunchParameters(ref NewUrlLaunchParameters_t data)
        {
            SuperfineSDK.Log(EventNames.NEW_URL_LAUNCH_PARAMETERS);
        }

        public static void LogAppProofOfPurchaseKeyResponse(ref AppProofOfPurchaseKeyResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "appId", data.m_nAppID);
            PutString(logData, "key", data.m_rgchKey);

            SuperfineSDK.Log(EventNames.APP_PROOF_OF_PURCHASE_KEY_RESPONSE, logData);
        }

        public static void LogFileDetailsResult(ref FileDetailsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "fileSize", data.m_ulFileSize);
            PutUint(logData, "flags", data.m_unFlags);
            PutByteArray(logData, "fileSHA", data.m_FileSHA);

            SuperfineSDK.Log(EventNames.FILE_DETAILS_RESULT, logData);
        }

        public static void LogTimedTrialStatus(ref TimedTrialStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID.m_AppId);
            PutBool(logData, "isOffline", data.m_bIsOffline);
            PutUint(logData, "secondsAllowed", data.m_unSecondsAllowed);
            PutUint(logData, "secondsPlayed", data.m_unSecondsPlayed);

            SuperfineSDK.Log(EventNames.TIMED_TRIAL_STATUS, logData);
        }

        public static void LogPersonaStateChange(ref PersonaStateChange_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_ulSteamID);
            PutInt(logData, "changeFlags", (int)data.m_nChangeFlags);

            SuperfineSDK.Log(EventNames.PERSONA_STATE_CHANGE, logData);
        }

        public static void LogGameOverlayActivated(ref GameOverlayActivated_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "active", data.m_bActive != 0);
            PutBool(logData, "userInitiated", data.m_bUserInitiated);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.GAME_OVERLAY_ACTIVATED, logData);
        }

        public static void LogGameServerChangeRequested(ref GameServerChangeRequested_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "server", data.m_rgchServer);
            PutString(logData, "password", data.m_rgchPassword);

            SuperfineSDK.Log(EventNames.GAME_SERVER_CHANGE_REQUESTED, logData);
        }

        public static void LogGameLobbyJoinRequested(ref GameLobbyJoinRequested_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_steamIDLobby.m_SteamID);
            PutUlong(logData, "friendSteamId", data.m_steamIDFriend.m_SteamID);

            SuperfineSDK.Log(EventNames.GAME_LOBBY_JOIN_REQUESTED, logData);
        }

        public static void LogAvatarImageLoaded(ref AvatarImageLoaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            PutInt(logData, "imageId", data.m_iImage);
            PutInt(logData, "wide", data.m_iWide);
            PutInt(logData, "tall", data.m_iTall);

            SuperfineSDK.Log(EventNames.AVATAR_IMAGE_LOADED, logData);
        }

        public static void LogClanOfficerListReponse(ref ClanOfficerListResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "clanSteamId", data.m_steamIDClan.m_SteamID);
            PutInt(logData, "officerCount", data.m_cOfficers);
            PutBool(logData, "success", data.m_bSuccess != 0);

            SuperfineSDK.Log(EventNames.CLAN_OFFICER_LIST_RESPONSE, logData);
        }

        public static void LogFriendRichPresenceUpdate(ref FriendRichPresenceUpdate_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "friendSteamId", data.m_steamIDFriend.m_SteamID);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.FRIEND_RICH_PRESENCE_UPDATE, logData);
        }

        public static void LogGameRichPresenceJoinRequested(ref GameRichPresenceJoinRequested_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "friendSteamId", data.m_steamIDFriend.m_SteamID);
            PutString(logData, "connectString", data.m_rgchConnect);

            SuperfineSDK.Log(EventNames.GAME_RICH_PRESENCE_JOIN_REQUESTED, logData);
        }

        public static void LogGameConnectedClanChatMsg(ref GameConnectedClanChatMsg_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "clanChatSteamId", data.m_steamIDClanChat.m_SteamID);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);
            PutInt(logData, "messageId", data.m_iMessageID);

            SuperfineSDK.Log(EventNames.GAME_CONNECTED_CLAN_CHAT_MSG, logData);
        }

        public static void LogGameConnectedChatJoin(ref GameConnectedChatJoin_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "clanChatSteamId", data.m_steamIDClanChat.m_SteamID);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.GAME_CONNECTED_CHAT_JOIN, logData);
        }

        public static void LogGameConnectedChatLeave(ref GameConnectedChatLeave_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "clanChatSteamId", data.m_steamIDClanChat.m_SteamID);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);
            PutBool(logData, "kicked", data.m_bKicked);
            PutBool(logData, "dropped", data.m_bDropped);

            SuperfineSDK.Log(EventNames.GAME_CONNECTED_CHAT_LEAVE, logData);
        }

        public static void LogDownloadClanActivityCountsResult(ref DownloadClanActivityCountsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "success", data.m_bSuccess);

            SuperfineSDK.Log(EventNames.DOWNLOAD_CLAN_ACTIVITY_COUNTS_RESULT, logData);
        }

        public static void LogJoinClanChatRoomCompletionResult(ref JoinClanChatRoomCompletionResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "clanChatSteamId", data.m_steamIDClanChat.m_SteamID);
            PutInt(logData, "chatRoomEnterResponse", (int)data.m_eChatRoomEnterResponse);

            SuperfineSDK.Log(EventNames.JOIN_CLAN_CHAT_ROOM_COMPLETION_RESULT, logData);
        }

        public static void LogGameConnectedFriendChatMsg(ref GameConnectedFriendChatMsg_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);
            PutInt(logData, "messageId", data.m_iMessageID);

            SuperfineSDK.Log(EventNames.GAME_CONNECTED_FRIEND_CHAT_MSG, logData);
        }

        public static void LogFriendsGetFollowerCount(ref FriendsGetFollowerCount_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            PutInt(logData, "count", data.m_nCount);

            SuperfineSDK.Log(EventNames.FRIENDS_GET_FOLLOWER_COUNT, logData);
        }

        public static void LogFriendsIsFollowing(ref FriendsIsFollowing_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            PutBool(logData, "isFollowing", data.m_bIsFollowing);

            SuperfineSDK.Log(EventNames.FRIENDS_IS_FOLLOWING, logData);
        }

        public static void LogFriendsEnumerateFollowingList(ref FriendsEnumerateFollowingList_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            if (data.m_rgSteamID != null)
            {
                int numSteamIds = Math.Min(data.m_rgSteamID.Length, data.m_nResultsReturned);
                if (numSteamIds > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numSteamIds; ++i)
                    {
                        arr.Add(data.m_rgSteamID[i].m_SteamID);
                    }

                    PutJSONArray(logData, "steamIds", arr);
                }
            }

            SuperfineSDK.Log(EventNames.FRIENDS_ENUMERATE_FOLLOWING_LIST, logData);
        }

        public static void LogSetPersonaNameResponse(ref SetPersonaNameResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_result);
            PutBool(logData, "success", data.m_bSuccess);
            PutBool(logData, "localSuccess", data.m_bLocalSuccess);

            SuperfineSDK.Log(EventNames.SET_PERSONA_NAME_RESPONSE, logData);
        }

        public static void LogUnreadChatMessagesChanged(ref UnreadChatMessagesChanged_t data)
        {
            SuperfineSDK.Log(EventNames.UNREAD_CHAT_MESSAGES_CHANGED);
        }

        public static void LogOverlayBrowserProtocolNavigation(ref OverlayBrowserProtocolNavigation_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "uri", data.rgchURI);
            SuperfineSDK.Log(EventNames.OVERLAY_BROWSER_PROTOCOL_NAVIGATION, logData);
        }

        public static void LogEquippedProfileItemsChanged(ref EquippedProfileItemsChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            SuperfineSDK.Log(EventNames.EQUIPPED_PROFILE_ITEMS_CHANGED, logData);
        }

        public static void LogEquippedProfileItems(ref EquippedProfileItems_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            PutBool(logData, "hasAnimatedAvatar", data.m_bHasAnimatedAvatar);
            PutBool(logData, "hasAvatarFrame", data.m_bHasAvatarFrame);
            PutBool(logData, "hasProfileModifier", data.m_bHasProfileModifier);
            PutBool(logData, "hasProfileBackground", data.m_bHasProfileBackground);
            PutBool(logData, "hasMiniProfileBackground", data.m_bHasMiniProfileBackground);

            SuperfineSDK.Log(EventNames.EQUIPPED_PROFILE_ITEMS, logData);
        }

        public static void LogGCMessageAvailable(ref GCMessageAvailable_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "messageSize", data.m_nMessageSize);
         
            SuperfineSDK.Log(EventNames.GC_MESSAGE_AVAILABLE, logData);
        }

        public static void LogGCMessageFailed(ref GCMessageFailed_t data)
        {
            SuperfineSDK.Log(EventNames.GC_MESSAGE_FAILED);
        }

        public static void LogGSClientApprove(ref GSClientApprove_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_SteamID.m_SteamID);
            PutUlong(logData, "ownerSteamId", data.m_OwnerSteamID.m_SteamID);

            SuperfineSDK.Log(EventNames.GS_CLIENT_APPROVE, logData);
        }

        public static void LogGSClientDeny(ref GSClientDeny_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_SteamID.m_SteamID);
            PutInt(logData, "denyReason", (int)data.m_eDenyReason);
            PutString(logData, "optionalText", data.m_rgchOptionalText);

            SuperfineSDK.Log(EventNames.GS_CLIENT_DENY, logData);
        }

        public static void LogGSClientKick(ref GSClientKick_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_SteamID.m_SteamID);
            PutInt(logData, "denyReason", (int)data.m_eDenyReason);

            SuperfineSDK.Log(EventNames.GS_CLIENT_KICK, logData);
        }

        public static void LogGSClientAchievementStatus(ref GSClientAchievementStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_SteamID);
            PutString(logData, "achievement", data.m_pchAchievement);
            PutBool(logData, "unlocked", data.m_bUnlocked);

            SuperfineSDK.Log(EventNames.GS_CLIENT_ACHIEVEMENT_STATUS, logData);
        }

        public static void LogGSPolicyResponse(ref GSPolicyResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "secure", data.m_bSecure != 0);

            SuperfineSDK.Log(EventNames.GS_POLICY_RESPONSE, logData);
        }

        public static void LogGSGameplayStats(ref GSGameplayStats_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "rank", data.m_nRank);
            PutUint(logData, "totalConnects", data.m_unTotalConnects);
            PutUint(logData, "totalMinutesPlayed", data.m_unTotalMinutesPlayed);

            SuperfineSDK.Log(EventNames.GS_GAMEPLAY_STATS, logData);
        }

        public static void LogGSClientGroupStatus(ref GSClientGroupStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "userSteamId", data.m_SteamIDUser.m_SteamID);
            PutUlong(logData, "groupSteamId", data.m_SteamIDGroup.m_SteamID);
            PutBool(logData, "member", data.m_bMember);
            PutBool(logData, "officer", data.m_bOfficer);

            SuperfineSDK.Log(EventNames.GS_CLIENT_GROUP_STATUS, logData);
        }

        public static void LogGSReputation(ref GSReputation_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "reputationScore", data.m_unReputationScore);
            PutBool(logData, "banned", data.m_bBanned);
            PutString(logData, "bannedIp", GetIPv4String(data.m_unBannedIP));
            PutUint(logData, "bannedPort", data.m_usBannedPort);
            PutUlong(logData, "bannedGameId", data.m_ulBannedGameID);
            PutUint(logData, "banExpires", data.m_unBanExpires);

            SuperfineSDK.Log(EventNames.GS_REPUTATION, logData);
        }

        public static void LogAssociateWithClanResult(ref AssociateWithClanResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.ASSOCIATE_WITH_CLAN_RESULT, logData);
        }

        public static void LogComputeNewPlayerCompatibilityResult(ref ComputeNewPlayerCompatibilityResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "playersThatDontLikeCandidate", data.m_cPlayersThatDontLikeCandidate);
            PutInt(logData, "playersThatCandidateDoesntLike", data.m_cPlayersThatCandidateDoesntLike);
            PutInt(logData, "clanPlayersThatDontLikeCandidate", data.m_cClanPlayersThatDontLikeCandidate);
            PutUlong(logData, "candidateSteamId", data.m_SteamIDCandidate.m_SteamID);

            SuperfineSDK.Log(EventNames.COMPUTE_NEW_PLAYER_COMPATIBILITY_RESULT, logData);
        }

        public static void LogGSStatsReceived(ref GSStatsReceived_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.GS_STATS_RECEIVED, logData);
        }

        public static void LogGSStatsStored(ref GSStatsStored_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.GS_STATS_STORED, logData);
        }

        public static void LogGSStatsUnloaded(ref GSStatsUnloaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.GS_STATS_UNLOADED, logData);
        }

        public static void LogHTMLBrowserReady(ref HTML_BrowserReady_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);

            SuperfineSDK.Log(EventNames.HTML_BROWSER_READY, logData);
        }

        public static void LogHTMLNeedsPaint(ref HTML_NeedsPaint_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "wide", data.unWide);
            PutUint(logData, "tall", data.unTall);
            PutUint(logData, "updateX", data.unUpdateX);
            PutUint(logData, "updateY", data.unUpdateY);
            PutUint(logData, "updateWide", data.unUpdateWide);
            PutUint(logData, "updateTall", data.unUpdateTall);
            PutUint(logData, "scrollX", data.unScrollX);
            PutUint(logData, "scrollY", data.unScrollY);
            PutFloat(logData, "pageScale", data.flPageScale);
            PutUint(logData, "pageSerial", data.unPageSerial);

            SuperfineSDK.Log(EventNames.HTML_NEEDS_PAINT, logData);
        }

        public static void LogHTMLStartRequest(ref HTML_StartRequest_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);
            PutString(logData, "target", data.pchTarget);
            PutString(logData, "postData", data.pchPostData);
            PutBool(logData, "isRedirect", data.bIsRedirect);

            SuperfineSDK.Log(EventNames.HTML_START_REQUEST, logData);
        }

        public static void LogHTMLCloseBrowser(ref HTML_CloseBrowser_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);

            SuperfineSDK.Log(EventNames.HTML_CLOSE_BROWSER, logData);
        }

        public static void LogHTMLURLChanged(ref HTML_URLChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);
            PutString(logData, "postData", data.pchPostData);
            PutBool(logData, "isRedirect", data.bIsRedirect);
            PutString(logData, "pageTitle", data.pchPageTitle);
            PutBool(logData, "newNavigation", data.bNewNavigation);

            SuperfineSDK.Log(EventNames.HTML_URL_CHANGED, logData);
        }

        public static void LogHTMLFinishedRequest(ref HTML_FinishedRequest_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);
            PutString(logData, "pageTitle", data.pchPageTitle);

            SuperfineSDK.Log(EventNames.HTML_FINISHED_REQUEST, logData);
        }

        public static void LogHTMLOpenLinkInNewTab(ref HTML_OpenLinkInNewTab_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);

            SuperfineSDK.Log(EventNames.HTML_OPEN_LINK_IN_NEW_TAB, logData);
        }

        public static void LogHTMLChangedTitle(ref HTML_ChangedTitle_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "title", data.pchTitle);

            SuperfineSDK.Log(EventNames.HTML_CHANGED_TITLE, logData);
        }

        public static void LogHTMLSearchResults(ref HTML_SearchResults_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "results", data.unResults);
            PutUint(logData, "currentMatch", data.unCurrentMatch);

            SuperfineSDK.Log(EventNames.HTML_SEARCH_RESULTS, logData);
        }

        public static void LogHTMLCanGobackAndForward(ref HTML_CanGoBackAndForward_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutBool(logData, "canGoBack", data.bCanGoBack);
            PutBool(logData, "canGoForward", data.bCanGoForward);

            SuperfineSDK.Log(EventNames.HTML_CAN_GO_BACK_AND_FORWARD, logData);
        }

        public static void LogHTMLHorizontalScroll(ref HTML_HorizontalScroll_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "scrollMax", data.unScrollMax);
            PutUint(logData, "scrollCurrent", data.unScrollCurrent);
            PutFloat(logData, "pageScale", data.flPageScale);
            PutBool(logData, "visible", data.bVisible);
            PutUint(logData, "pageSize", data.unPageSize);

            SuperfineSDK.Log(EventNames.HTML_HORIZONTAL_SCROLL, logData);
        }

        public static void LogHTMLVerticalScroll(ref HTML_VerticalScroll_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "scrollMax", data.unScrollMax);
            PutUint(logData, "scrollCurrent", data.unScrollCurrent);
            PutFloat(logData, "pageScale", data.flPageScale);
            PutBool(logData, "visible", data.bVisible);
            PutUint(logData, "pageSize", data.unPageSize);

            SuperfineSDK.Log(EventNames.HTML_VERTICAL_SCROLL, logData);
        }

        public static void LogHTMLLinkAtPosition(ref HTML_LinkAtPosition_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);
            PutBool(logData, "input", data.bInput);
            PutBool(logData, "liveLink", data.bLiveLink);

            SuperfineSDK.Log(EventNames.HTML_LINK_AT_POSITION, logData);
        }

        public static void LogHTMLJSAlert(ref HTML_JSAlert_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "message", data.pchMessage);

            SuperfineSDK.Log(EventNames.HTML_JS_ALERT, logData);
        }

        public static void LogHTMLJSConfirm(ref HTML_JSConfirm_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "message", data.pchMessage);

            SuperfineSDK.Log(EventNames.HTML_JS_CONFIRM, logData);
        }

        public static void LogHTMLFileOpenDialog(ref HTML_FileOpenDialog_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "title", data.pchTitle);
            PutString(logData, "initialFile", data.pchInitialFile);

            SuperfineSDK.Log(EventNames.HTML_FILE_OPEN_DIALOG, logData);
        }

        public static void LogHTMLNewWindow(ref HTML_NewWindow_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "url", data.pchURL);
            PutUint(logData, "x", data.unX);
            PutUint(logData, "y", data.unY);
            PutUint(logData, "wide", data.unWide);
            PutUint(logData, "tall", data.unTall);

            SuperfineSDK.Log(EventNames.HTML_NEW_WINDOW, logData);
        }

        public static void LogHTMLSetCursor(ref HTML_SetCursor_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "mouseCursor", data.eMouseCursor);

            SuperfineSDK.Log(EventNames.HTML_SET_CURSOR, logData);
        }

        public static void LogHTMLStatusText(ref HTML_StatusText_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "msg", data.pchMsg);

            SuperfineSDK.Log(EventNames.HTML_STATUS_TEXT, logData);
        }

        public static void LogHTMLShowToolTip(ref HTML_ShowToolTip_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "msg", data.pchMsg);

            SuperfineSDK.Log(EventNames.HTML_SHOW_TOOL_TIP, logData);
        }

        public static void LogHTMLUpdateToolTip(ref HTML_UpdateToolTip_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutString(logData, "msg", data.pchMsg);

            SuperfineSDK.Log(EventNames.HTML_UPDATE_TOOL_TIP, logData);
        }

        public static void LogHTMLHideToolTip(ref HTML_HideToolTip_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);

            SuperfineSDK.Log(EventNames.HTML_HIDE_TOOL_TIP, logData);
        }

        public static void LogHTMLBrowserRestarted(ref HTML_BrowserRestarted_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "browserHandle", data.unBrowserHandle.m_HHTMLBrowser);
            PutUint(logData, "oldBrowserHandle", data.unOldBrowserHandle.m_HHTMLBrowser);

            SuperfineSDK.Log(EventNames.HTML_BROWSER_RESTARTED, logData);
        }

        public static void LogHTTPRequestCompleted(ref HTTPRequestCompleted_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "request", data.m_hRequest.m_HTTPRequestHandle);
            PutUlong(logData, "contextValue", data.m_ulContextValue);
            PutBool(logData, "requestSuccessful", data.m_bRequestSuccessful);
            PutInt(logData, "statusCode", (int)data.m_eStatusCode);
            PutUint(logData, "bodySize", data.m_unBodySize);

            SuperfineSDK.Log(EventNames.HTTP_REQUEST_COMPLETED, logData);
        }

        public static void LogHTTPRequestHeadersReceived(ref HTTPRequestHeadersReceived_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "request", data.m_hRequest.m_HTTPRequestHandle);
            PutUlong(logData, "contextValue", data.m_ulContextValue);

            SuperfineSDK.Log(EventNames.HTTP_REQUEST_HEADERS_RECEIVED, logData);
        }

        public static void LogHTTPRequestDataReceived(ref HTTPRequestDataReceived_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "request", data.m_hRequest.m_HTTPRequestHandle);
            PutUlong(logData, "contextValue", data.m_ulContextValue);
            PutUint(logData, "offset", data.m_cOffset);
            PutUint(logData, "bytesReceived", data.m_cBytesReceived);

            SuperfineSDK.Log(EventNames.HTTP_REQUEST_DATA_RECEIVED, logData);
        }

        public static void LogInputDeviceConnected(ref SteamInputDeviceConnected_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "connectedDeviceHandle", data.m_ulConnectedDeviceHandle.m_InputHandle);

            SuperfineSDK.Log(EventNames.INPUT_DEVICE_CONNECTED, logData);
        }

        public static void LogInputDeviceDisconnected(ref SteamInputDeviceDisconnected_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "disconnectedDeviceHandle", data.m_ulDisconnectedDeviceHandle.m_InputHandle);

            SuperfineSDK.Log(EventNames.INPUT_DEVICE_DISCONNECTED, logData);
        }

        public static void LogInputConfigurationLoaded(ref SteamInputConfigurationLoaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID.m_AppId);
            PutUlong(logData, "deviceHandle", data.m_ulDeviceHandle.m_InputHandle);
            PutUlong(logData, "mappingCreator", data.m_ulMappingCreator.m_SteamID);
            PutUint(logData, "majorRevision", data.m_unMajorRevision);
            PutUint(logData, "minorRevision", data.m_unMinorRevision);
            PutBool(logData, "usesSteamInputApi", data.m_bUsesSteamInputAPI);
            PutBool(logData, "usesGamepadApi", data.m_bUsesGamepadAPI);

            SuperfineSDK.Log(EventNames.INPUT_CONFIGURATION_LOADED, logData);
        }

        public static void LogInputGamepadSlotChange(ref SteamInputGamepadSlotChange_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID.m_AppId);
            PutUlong(logData, "deviceHandle", data.m_ulDeviceHandle.m_InputHandle);
            PutInt(logData, "deviceType", (int)data.m_eDeviceType);
            PutInt(logData, "oldGamepadSlot", data.m_nOldGamepadSlot);
            PutInt(logData, "newGamepadSlot", data.m_nNewGamepadSlot);

            SuperfineSDK.Log(EventNames.INPUT_GAMEPAD_SLOT_CHANGE, logData);
        }

        public static void LogInventoryResultReady(ref SteamInventoryResultReady_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "handle", data.m_handle.m_SteamInventoryResult);
            PutInt(logData, "result", (int)data.m_result);

            SuperfineSDK.Log(EventNames.INVENTORY_RESULT_READY, logData);
        }

        public static void LogInventoryFullUpdate(ref SteamInventoryFullUpdate_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "handle", data.m_handle.m_SteamInventoryResult);

            SuperfineSDK.Log(EventNames.INVENTORY_FULL_UPDATE, logData);
        }

        public static void LogInventoryDefinitionUpdate(ref SteamInventoryDefinitionUpdate_t data)
        {
            SuperfineSDK.Log(EventNames.INVENTORY_DEFINITION_UPDATE);
        }

        public static void LogInventoryEligiblePromoItemDefIDs(ref SteamInventoryEligiblePromoItemDefIDs_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_result);
            PutUlong(logData, "steamId", data.m_steamID.m_SteamID);
            PutInt(logData, "numEligiblePromoItemDefs", data.m_numEligiblePromoItemDefs);
            PutBool(logData, "cachedData", data.m_bCachedData);

            SuperfineSDK.Log(EventNames.INVENTORY_ELIGIBLE_PROMO_ITEM_DEF_IDS, logData);
        }

        public static void LogInventoryStartPurchaseResult(ref SteamInventoryStartPurchaseResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_result);
            PutUlong(logData, "orderId", data.m_ulOrderID);
            PutUlong(logData, "transId", data.m_ulTransID);

            SuperfineSDK.Log(EventNames.INVENTORY_START_PURCHASE_RESULT, logData);
        }

        public static void LogInventoryRequestPricesResult(ref SteamInventoryRequestPricesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_result);
            PutString(logData, "currency", data.m_rgchCurrency);

            SuperfineSDK.Log(EventNames.INVENTORY_REQUEST_PRICES_RESULT, logData);
        }

        public static void LogFavoritesListChanged(ref FavoritesListChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "ip", GetIPv4String(data.m_nIP));
            PutUint(logData, "queryPort", data.m_nQueryPort);
            PutUint(logData, "connPort", data.m_nConnPort);
            PutUint(logData, "appId", data.m_nAppID);
            PutUint(logData, "flags", data.m_nFlags);
            PutBool(logData, "add", data.m_bAdd);
            PutUint(logData, "accountId", data.m_unAccountId.m_AccountID);

            SuperfineSDK.Log(EventNames.FAVORITES_LIST_CHANGED, logData);
        }

        public static void LogLobbyInvite(ref LobbyInvite_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "userSteamId", data.m_ulSteamIDUser);
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "gameId", data.m_ulGameID);

            SuperfineSDK.Log(EventNames.LOBBY_INVITE, logData);
        }

        public static void LogLobbyEnter(ref LobbyEnter_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUint(logData, "chatPermissions", data.m_rgfChatPermissions);
            PutBool(logData, "locked", data.m_bLocked);
            PutUint(logData, "chatRoomEnterResponse", data.m_EChatRoomEnterResponse);

            SuperfineSDK.Log(EventNames.LOBBY_ENTER, logData);
        }

        public static void LogLobbyDataUpdate(ref LobbyDataUpdate_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "memberSteamId", data.m_ulSteamIDMember);
            PutBool(logData, "success", data.m_bSuccess != 0);

            SuperfineSDK.Log(EventNames.LOBBY_DATA_UPDATE, logData);
        }

        public static void LogLobbyChatUpdate(ref LobbyChatUpdate_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "userChangedSteamId", data.m_ulSteamIDUserChanged);
            PutUlong(logData, "makingChangeSteamId", data.m_ulSteamIDMakingChange);
            PutUint(logData, "chatMemberStateChange", data.m_rgfChatMemberStateChange);

            SuperfineSDK.Log(EventNames.LOBBY_CHAT_UPDATE, logData);
        }

        public static void LogLobbyChatMsg(ref LobbyChatMsg_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "userSteamId", data.m_ulSteamIDUser);
            PutByte(logData, "chatEntryType", data.m_eChatEntryType);
            PutUint(logData, "chatId", data.m_iChatID);

            SuperfineSDK.Log(EventNames.LOBBY_CHAT_MSG, logData);
        }

        public static void LogLobbyGameCreated(ref LobbyGameCreated_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "gameServerSteamId", data.m_ulSteamIDGameServer);
            PutString(logData, "ip", GetIPv4String(data.m_unIP));
            PutUint(logData, "port", data.m_usPort);

            SuperfineSDK.Log(EventNames.LOBBY_GAME_CREATED, logData);
        }

        public static void LogLobbyMatchList(ref LobbyMatchList_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "lobbiesMatching", data.m_nLobbiesMatching);

            SuperfineSDK.Log(EventNames.LOBBY_MATCH_LIST, logData);
        }

        public static void LogLobbyKicked(ref LobbyKicked_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);
            PutUlong(logData, "adminSteamId", data.m_ulSteamIDAdmin);
            PutBool(logData, "kickedDueToDisconnect", data.m_bKickedDueToDisconnect != 0);

            SuperfineSDK.Log(EventNames.LOBBY_KICKED, logData);
        }

        public static void LogLobbyCreated(ref LobbyCreated_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "lobbySteamId", data.m_ulSteamIDLobby);

            SuperfineSDK.Log(EventNames.LOBBY_CREATED, logData);
        }

        /*
        public static void LogPSNGameBootInviteResult(ref PSNGameBootInviteResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "gameBootInviteExists", data.m_bGameBootInviteExists);
            PutUlong(logData, "lobbySteamId", data.m_steamIDLobby.m_SteamID);

            SuperfineSDK.Log(EventNames.PSN_GAME_BOOT_INVITE_RESULT, logData);
        }
        */

        public static void LogFavoritesListAccountsUpdated(ref FavoritesListAccountsUpdated_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.FAVORITES_LIST_ACCOUNTS_UPDATED, logData);
        }

        public static void LogSearchForGameProgressCallback(ref SearchForGameProgressCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "searchId", data.m_ullSearchID);
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "lobbySteamId", data.m_lobbyID.m_SteamID);
            PutUlong(logData, "endedSearchSteamId", data.m_steamIDEndedSearch.m_SteamID);
            PutInt(logData, "secondsRemainingEstimate", data.m_nSecondsRemainingEstimate);
            PutInt(logData, "playersSearching", data.m_cPlayersSearching);

            SuperfineSDK.Log(EventNames.SEARCH_FOR_GAME_PROGRESS_CALLBACK, logData);
        }

        public static void LogSearchForGameResultCallback(ref SearchForGameResultCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "searchId", data.m_ullSearchID);
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "playersInGameCount", data.m_nCountPlayersInGame);
            PutInt(logData, "acceptedGameCount", data.m_nCountAcceptedGame);
            PutUlong(logData, "hostSteamId", data.m_steamIDHost.m_SteamID);
            PutBool(logData, "finalCallback", data.m_bFinalCallback);

            SuperfineSDK.Log(EventNames.SEARCH_FOR_GAME_RESULT_CALLBACK, logData);
        }

        public static void LogRequestPlayersForGameProgressCallback(ref RequestPlayersForGameProgressCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "searchId", data.m_ullSearchID);

            SuperfineSDK.Log(EventNames.REQUEST_PLAYERS_FOR_GAME_PROGRESS_CALLBACK, logData);
        }

        public static void LogRequestPlayersForGameResultCallback(ref RequestPlayersForGameResultCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "searchId", data.m_ullSearchID);
            PutUlong(logData, "playerFoundSteamId", data.m_SteamIDPlayerFound.m_SteamID);
            PutUlong(logData, "lobbySteamId", data.m_SteamIDLobby.m_SteamID);
            PutInt(logData, "playerAcceptState", (int)data.m_ePlayerAcceptState);
            PutInt(logData, "playerIndex", data.m_nPlayerIndex);
            PutInt(logData, "totalPlayersFound", data.m_nTotalPlayersFound);
            PutInt(logData, "totalPlayersAcceptedGame", data.m_nTotalPlayersAcceptedGame);
            PutInt(logData, "suggestedTeamIndex", data.m_nSuggestedTeamIndex);
            PutUlong(logData, "uniqueGameId", data.m_ullUniqueGameID);

            SuperfineSDK.Log(EventNames.REQUEST_PLAYERS_FOR_GAME_RESULT_CALLBACK, logData);
        }

        public static void LogRequestPlayersForGameFinalResultCallback(ref RequestPlayersForGameFinalResultCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "searchId", data.m_ullSearchID);
            PutUlong(logData, "uniqueGameId", data.m_ullUniqueGameID);

            SuperfineSDK.Log(EventNames.REQUEST_PLAYERS_FOR_GAME_FINAL_RESULT_CALLBACK, logData);
        }

        public static void LogSubmitPlayerResultCallback(ref SubmitPlayerResultResultCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "uniqueGameId", data.ullUniqueGameID);
            PutUlong(logData, "playerSteamId", data.steamIDPlayer.m_SteamID);

            SuperfineSDK.Log(EventNames.SUBMIT_PLAYER_RESULT_CALLBACK, logData);
        }

        public static void LogEndGameResultCallback(ref EndGameResultCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "uniqueGameId", data.ullUniqueGameID);

            SuperfineSDK.Log(EventNames.END_GAME_RESULT_CALLBACK, logData);
        }

        public static void LogJoinPartyCallback(ref JoinPartyCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "partyBeaconId", data.m_ulBeaconID.m_PartyBeaconID);
            PutUlong(logData, "beaconOwnerSteamId", data.m_SteamIDBeaconOwner.m_SteamID);
            PutString(logData, "connectString", data.m_rgchConnectString);

            SuperfineSDK.Log(EventNames.JOIN_PARTY_CALLBACK, logData);
        }

        public static void LogCreateBeaconCallback(ref CreateBeaconCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "beaconId", data.m_ulBeaconID.m_PartyBeaconID);

            SuperfineSDK.Log(EventNames.CREATE_BEACON_CALLBACK, logData);
        }

        public static void LogReservationNotificationCallback(ref ReservationNotificationCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "beaconId", data.m_ulBeaconID.m_PartyBeaconID);
            PutUlong(logData, "joinerSteamId", data.m_steamIDJoiner.m_SteamID);

            SuperfineSDK.Log(EventNames.RESERVATION_NOTIFICATION_CALLBACK, logData);
        }

        public static void LogChangeNumOpenSlotsCallback(ref ChangeNumOpenSlotsCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.CHANGE_NUM_OPEN_SLOTS_CALLBACK, logData);
        }

        public static void LogAvailableBeaconLocationsUpdated(ref AvailableBeaconLocationsUpdated_t data)
        {
            SuperfineSDK.Log(EventNames.AVAILABLE_BEACON_LOCATIONS_UPDATED);
        }

        public static void LogActiveBeaconsUpdated(ref ActiveBeaconsUpdated_t data)
        {
            SuperfineSDK.Log(EventNames.ACTIVE_BEACONS_UPDATED);
        }

        public static void LogPlaybackStatusHasChanged(ref PlaybackStatusHasChanged_t data)
        {
            SuperfineSDK.Log(EventNames.PLAYBACK_STATUS_HAS_CHANGED);
        }

        public static void LogVolumeHasChanged(ref VolumeHasChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutFloat(logData, "newVolume", data.m_flNewVolume);

            SuperfineSDK.Log(EventNames.VOLUME_HAS_CHANGED, logData);
        }

        public static void LogMusicPlayerRemoteWillActivate(ref MusicPlayerRemoteWillActivate_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_REMOTE_WILL_ACTIVATE);
        }

        public static void LogMusicPlayerRemoteWillDeactivate(ref MusicPlayerRemoteWillDeactivate_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_REMOTE_WILL_DEACTIVATE);
        }

        public static void LogMusicPlayerRemoteToFront(ref MusicPlayerRemoteToFront_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_REMOTE_TO_FRONT);
        }

        public static void LogMusicPlayerWillQuit(ref MusicPlayerWillQuit_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WILL_QUIT);
        }

        public static void LogMusicPlayerWantsPlay(ref MusicPlayerWantsPlay_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_PLAY);
        }

        public static void LogMusicPlayerWantsPause(ref MusicPlayerWantsPause_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_PAUSE);
        }

        public static void LogMusicPlayerWantsPlayPrevious(ref MusicPlayerWantsPlayPrevious_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_PLAY_PREVIOUS);
        }

        public static void LogMusicPlayerWantsPlayNext(ref MusicPlayerWantsPlayNext_t data)
        {
            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_PLAY_NEXT);
        }

        public static void LogMusicPlayerWantsShuffled(ref MusicPlayerWantsShuffled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "shuffled", data.m_bShuffled);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_SHUFFLED, logData);
        }

        public static void LogMusicPlayerWantsLooped(ref MusicPlayerWantsLooped_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "looped", data.m_bLooped);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_LOOPED, logData);
        }

        public static void LogMusicPlayerWantsVolume(ref MusicPlayerWantsVolume_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutFloat(logData, "newVolume", data.m_flNewVolume);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_VOLUME, logData);
        }

        public static void LogMusicPlayerSelectsQueueEntry(ref MusicPlayerSelectsQueueEntry_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "id", data.nID);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_SELECTS_QUEUE_ENTRY, logData);
        }

        public static void LogMusicPlayerSelectsPlaylistEntry(ref MusicPlayerSelectsPlaylistEntry_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "id", data.nID);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_SELECTS_PLAYLIST_ENTRY, logData);
        }

        public static void LogMusicPlayerWantsPlayingRepeatStatus(ref MusicPlayerWantsPlayingRepeatStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "playingRepeatStatus", data.m_nPlayingRepeatStatus);

            SuperfineSDK.Log(EventNames.MUSIC_PLAYER_WANTS_PLAYING_REPEAT_STATUS, logData);
        }

        public static void LogP2PSessionRequest(ref P2PSessionRequest_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "remoteSteamId", data.m_steamIDRemote.m_SteamID);

            SuperfineSDK.Log(EventNames.P2P_SESSION_REQUEST, logData);
        }

        public static void LogP2PSessionConnectFail(ref P2PSessionConnectFail_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "remoteSteamId", data.m_steamIDRemote.m_SteamID);
            PutByte(logData, "p2pSessionError", data.m_eP2PSessionError);

            SuperfineSDK.Log(EventNames.P2P_SESSION_CONNECT_FAIL, logData);
        }

        public static void LogSocketStatusCallback(ref SocketStatusCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "socket", data.m_hSocket.m_SNetSocket);
            PutUint(logData, "listenSocket", data.m_hListenSocket.m_SNetListenSocket);
            PutUlong(logData, "remoteSteamId", data.m_steamIDRemote.m_SteamID);
            PutInt(logData, "socketState", data.m_eSNetSocketState);

            SuperfineSDK.Log(EventNames.SOCKET_STATUS_CALLBACK, logData);
        }

        public static void LogNetworkingMessagesSessionRequest(ref SteamNetworkingMessagesSessionRequest_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutJSONObject(logData, "remoteIdentity", GetJSONObjectFromSteamNetworkingIdentity(ref data.m_identityRemote));

            SuperfineSDK.Log(EventNames.NETWORKING_MESSAGES_SESSION_REQUEST, logData);
        }

        public static void LogNetworkingMessagesSessionFailed(ref SteamNetworkingMessagesSessionFailed_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutJSONObject(logData, "info", GetJSONObjectFromSteamNetConnectionInfo(ref data.m_info));

            SuperfineSDK.Log(EventNames.NETWORKING_MESSAGES_SESSION_FAILED, logData);
        }

        public static void LogNetConnectionStatusChangedCallback(ref SteamNetConnectionStatusChangedCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "conn", data.m_hConn.m_HSteamNetConnection);
            PutJSONObject(logData, "info", GetJSONObjectFromSteamNetConnectionInfo(ref data.m_info));
            PutInt(logData, "oldState", (int)data.m_eOldState);

            SuperfineSDK.Log(EventNames.NET_CONNECTION_STATUS_CHANGED_CALLBACK, logData);
        }

        public static void LogNetAuthenticationStatus(ref SteamNetAuthenticationStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "avail", (int)data.m_eAvail);
            PutString(logData, "debugMsg", data.m_debugMsg);

            SuperfineSDK.Log(EventNames.NET_AUTHENTICATION_STATUS, logData);
        }

        public static void LogNetworkingFakeIPResult(ref SteamNetworkingFakeIPResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutJSONObject(logData, "identity", GetJSONObjectFromSteamNetworkingIdentity(ref data.m_identity));
            PutString(logData, "ip", GetIPv4String(data.m_unIP));

            ushort[] ports = data.m_unPorts;
            if (ports != null)
            {
                int numPorts = ports.Length;
                if (numPorts > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numPorts; ++i)
                    {
                        int port = ports[i];
                        if (port == 0) break;

                        arr.Add(port);
                    }

                    if (arr.Count > 0)
                    {
                        PutJSONArray(logData, "ports", arr);
                    }
                }
            }

            SuperfineSDK.Log(EventNames.NETWORKING_FAKE_IP_RESULT, logData);
        }

        public static void LogRelayNetworkStatus(ref SteamRelayNetworkStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "avail", (int)data.m_eAvail);
            PutBool(logData, "pingMeasurementInProgress", data.m_bPingMeasurementInProgress != 0);
            PutInt(logData, "networkConfigAvail", (int)data.m_eAvailNetworkConfig);
            PutInt(logData, "anyRelayAvail", (int)data.m_eAvailAnyRelay);

            PutString(logData, "debugMsg", data.m_debugMsg);

            SuperfineSDK.Log(EventNames.RELAY_NETWORK_STATUS, logData);
        }

        public static void LogParentalSettingsChanged(ref SteamParentalSettingsChanged_t data)
        {
            SuperfineSDK.Log(EventNames.PARENTAL_SETTINGS_CHANGED);
        }

        public static void LogRemotePlaySessionConnected(ref SteamRemotePlaySessionConnected_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "sessionId", data.m_unSessionID.m_RemotePlaySessionID);

            SuperfineSDK.Log(EventNames.REMOTE_PLAY_SESSION_CONNECTED, logData);
        }

        public static void LogRemotePlaySessionDisconnected(ref SteamRemotePlaySessionDisconnected_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "sessionId", data.m_unSessionID.m_RemotePlaySessionID);

            SuperfineSDK.Log(EventNames.REMOTE_PLAY_SESSION_DISCONNECTED, logData);
        }

        public static void LogRemotePlayTogetherGuestInvite(ref SteamRemotePlayTogetherGuestInvite_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "connectUrl", data.m_szConnectURL);

            SuperfineSDK.Log(EventNames.REMOTE_PLAY_TOGETHER_GUEST_INVITE, logData);
        }

        public static void LogRemoteStorageFileShareResult(ref RemoteStorageFileShareResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "file", data.m_hFile.m_UGCHandle);
            PutString(logData, "filename", data.m_rgchFilename);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_FILE_SHARE_RESULT, logData);
        }

        public static void LogRemoteStoragePublishFileResult(ref RemoteStoragePublishFileResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutBool(logData, "userNeedsToAcceptWorkshopLegalAgreement", data.m_bUserNeedsToAcceptWorkshopLegalAgreement);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISH_FILE_RESULT, logData);
        }

        public static void LogRemoteStorageDeletePublishedFileResult(ref RemoteStorageDeletePublishedFileResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_DELETE_PUBLISHED_FILE_RESULT, logData);
        }

        public static void LogRemoteStorageEnumerateUserPublishedFilesResult(ref RemoteStorageEnumerateUserPublishedFilesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            PublishedFileId_t[] fileIds = data.m_rgPublishedFileId;
            if (fileIds != null)
            {
                int numFiles = Math.Min(fileIds.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(fileIds[i].m_PublishedFileId);
                    }

                    PutJSONArray(logData, "publishedFileIds", arr);
                }
            }

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_ENUMERATE_USER_PUBLISHED_FILES_RESULT, logData);
        }

        public static void LogRemoteStorageSubscribePublishedFileResult(ref RemoteStorageSubscribePublishedFileResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_SUBSCRIBE_PUBLISHED_FILE_RESULT, logData);
        }

        public static void LogRemoteStorageEnumerateUserSubscribedFilesResult(ref RemoteStorageEnumerateUserSubscribedFilesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            PublishedFileId_t[] fileIds = data.m_rgPublishedFileId;
            if (fileIds != null)
            {
                int numFiles = Math.Min(fileIds.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(fileIds[i].m_PublishedFileId);
                    }

                    PutJSONArray(logData, "publishedFileIds", arr);
                }
            }

            uint[] subscribedTimes = data.m_rgRTimeSubscribed;
            if (subscribedTimes != null)
            {
                int numFiles = Math.Min(subscribedTimes.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(subscribedTimes[i]);
                    }

                    PutJSONArray(logData, "subscribedTimes", arr);
                }
            }

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_ENUMERATE_USER_SUBSCRIBED_FILES_RESULT, logData);
        }

        public static void LogRemoteStorageUnsubscribePublishedFileResult(ref RemoteStorageUnsubscribePublishedFileResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_UNSUBSCRIBE_PUBLISHED_FILE_RESULT, logData);
        }

        public static void LogRemoteStorageUpdatePublishedFileResult(ref RemoteStorageUpdatePublishedFileResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutBool(logData, "userNeedsToAcceptWorkshopLegalAgreement", data.m_bUserNeedsToAcceptWorkshopLegalAgreement);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_UPDATE_PUBLISHED_FILE_RESULT, logData);
        }

        public static void LogRemoteStorageDownloadUGCResult(ref RemoteStorageDownloadUGCResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "file", data.m_hFile.m_UGCHandle);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);
            PutInt(logData, "sizeInBytes", data.m_nSizeInBytes);
            PutString(logData, "filename", data.m_pchFileName);
            PutUlong(logData, "ownerSteamId", data.m_ulSteamIDOwner);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_DOWNLOAD_UGC_RESULT, logData);
        }

        public static void LogRemoteStorageGetPublishedFileDetailsResult(ref RemoteStorageGetPublishedFileDetailsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "creatorAppId", data.m_nCreatorAppID.m_AppId);
            PutUint(logData, "consumerAppId", data.m_nConsumerAppID.m_AppId);
            PutString(logData, "title", data.m_rgchTitle);
            PutString(logData, "description", data.m_rgchDescription);
            PutUlong(logData, "file", data.m_hFile.m_UGCHandle);
            PutUlong(logData, "previewFile", data.m_hPreviewFile.m_UGCHandle);
            PutUlong(logData, "ownerSteamId", data.m_ulSteamIDOwner);
            PutUint(logData, "createdTime", data.m_rtimeCreated);
            PutUint(logData, "updatedTime", data.m_rtimeUpdated);
            PutInt(logData, "visibility", (int)data.m_eVisibility);
            PutBool(logData, "banned", data.m_bBanned);
            PutString(logData, "tags", data.m_rgchTags);
            PutBool(logData, "tagsTruncated", data.m_bTagsTruncated);
            PutString(logData, "filename", data.m_pchFileName);
            PutInt(logData, "fileSize", data.m_nFileSize);
            PutInt(logData, "previewFileSize", data.m_nPreviewFileSize);
            PutString(logData, "url", data.m_rgchURL);
            PutInt(logData, "fileType", (int)data.m_eFileType);
            PutBool(logData, "acceptedForUse", data.m_bAcceptedForUse);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_GET_PUBLISHED_FILE_DETAILS_RESULT, logData);
        }

        public static void LogRemoteStorageEnumerateWorkshopFilesResult(ref RemoteStorageEnumerateWorkshopFilesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            PublishedFileId_t[] fileIds = data.m_rgPublishedFileId;
            if (fileIds != null)
            {
                int numFiles = Math.Min(fileIds.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(fileIds[i].m_PublishedFileId);
                    }

                    PutJSONArray(logData, "publishedFileIds", arr);
                }
            }

            float[] scores = data.m_rgScore;
            if (scores != null)
            {
                int numFiles = Math.Min(scores.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(scores[i]);
                    }

                    PutJSONArray(logData, "scores", arr);
                }
            }

            PutUint(logData, "appId", data.m_nAppId.m_AppId);
            PutUint(logData, "startIndex", data.m_unStartIndex);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_ENUMERATE_WORKSHOP_FILES_RESULT, logData);
        }

        public static void LogRemoteStorageGetPublishedItemVoteDetailsResult(ref RemoteStorageGetPublishedItemVoteDetailsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_unPublishedFileId.m_PublishedFileId);
            PutInt(logData, "votesFor", data.m_nVotesFor);
            PutInt(logData, "votesAgainst", data.m_nVotesAgainst);
            PutInt(logData, "reports", data.m_nReports);
            PutFloat(logData, "score", data.m_fScore);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_GET_PUBLISHED_ITEM_VOTE_DETAILS_RESULT, logData);
        }

        public static void LogRemoteStoragePublishedFileSubscribed(ref RemoteStoragePublishedFileSubscribed_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);
            
            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISHED_FILE_SUBSCRIBED, logData);
        }

        public static void LogRemoteStoragePublishedFileUnsubscribed(ref RemoteStoragePublishedFileUnsubscribed_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISHED_FILE_UNSUBSCRIBED, logData);
        }

        public static void LogRemoteStoragePublishedFileDeleted(ref RemoteStoragePublishedFileDeleted_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISHED_FILE_DELETED, logData);
        }

        public static void LogRemoteStorageUpdateUserPublishedItemVoteResult(ref RemoteStorageUpdateUserPublishedItemVoteResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_UPDATE_USER_PUBLISHED_ITEM_VOTE_RESULT, logData);
        }

        public static void LogRemoteStorageUserVoteDetails(ref RemoteStorageUserVoteDetails_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "vote", (int)data.m_eVote);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_USER_VOTE_DETAILS, logData);
        }

        public static void LogRemoteStorageEnumerateUserSharedWorkshopFilesResult(ref RemoteStorageEnumerateUserSharedWorkshopFilesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            PublishedFileId_t[] fileIds = data.m_rgPublishedFileId;
            if (fileIds != null)
            {
                int numFiles = Math.Min(fileIds.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(fileIds[i].m_PublishedFileId);
                    }

                    PutJSONArray(logData, "publishedFileIds", arr);
                }
            }

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_ENUMERATE_USER_SHARED_WORKSHOP_FILES_RESULT, logData);
        }

        public static void LogRemoteStorageSetUserPublishedFileActionResult(ref RemoteStorageSetUserPublishedFileActionResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "action", (int)data.m_eAction);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_SET_USER_PUBLISHED_FILE_ACTION_RESULT, logData);
        }

        public static void LogRemoteStorageEnumeratePublishedFilesByUserActionResult(ref RemoteStorageEnumeratePublishedFilesByUserActionResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutInt(logData, "action", (int)data.m_eAction);
            PutInt(logData, "totalResultCount", data.m_nTotalResultCount);

            PublishedFileId_t[] fileIds = data.m_rgPublishedFileId;
            if (fileIds != null)
            {
                int numFiles = Math.Min(fileIds.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(fileIds[i].m_PublishedFileId);
                    }

                    PutJSONArray(logData, "publishedFileIds", arr);
                }
            }

            uint[] updatedTimes = data.m_rgRTimeUpdated;
            if (updatedTimes != null)
            {
                int numFiles = Math.Min(updatedTimes.Length, data.m_nResultsReturned);
                if (numFiles > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numFiles; ++i)
                    {
                        arr.Add(updatedTimes[i]);
                    }

                    PutJSONArray(logData, "updatedTimes", arr);
                }
            }

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_ENUMERATE_PUBLISHED_FILES_BY_USER_ACTION_RESULT, logData);
        }

        public static void LogRemoteStoragePublishFileProgress(ref RemoteStoragePublishFileProgress_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutDouble(logData, "filePercent", data.m_dPercentFile);
            PutBool(logData, "preview", data.m_bPreview);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISH_FILE_PROGRESS, logData);
        }

        public static void LogRemoteStoragePublishedFileUpdated(ref RemoteStoragePublishedFileUpdated_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_PUBLISHED_FILE_UPDATED, logData);
        }

        public static void LogRemoteStorageFileWriteAsyncComplete(ref RemoteStorageFileWriteAsyncComplete_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_FILE_WRITE_ASYNC_COMPLETE, logData);
        }

        public static void LogRemoteStorageFileReadAsyncComplete(ref RemoteStorageFileReadAsyncComplete_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "fileReadAsyncCall", data.m_hFileReadAsync.m_SteamAPICall);
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "offset", data.m_nOffset);
            PutUint(logData, "read", data.m_cubRead);

            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_FILE_READ_ASYNC_COMPLETE, logData);
        }

        public static void LogRemoteStorageLocalFileChange(ref RemoteStorageLocalFileChange_t data)
        {
            SuperfineSDK.Log(EventNames.REMOTE_STORAGE_LOCAL_FILE_CHANGE);
        }

        public static void LogScreenshotReady(ref ScreenshotReady_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "local", data.m_hLocal.m_ScreenshotHandle);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.SCREENSHOT_READY, logData);
        }

        public static void LogScreenshotRequested(ref ScreenshotRequested_t data)
        {
            SuperfineSDK.Log(EventNames.SCREENSHOT_REQUESTED);
        }

        public static void LogUGCQueryCompleted(ref SteamUGCQueryCompleted_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "handle", data.m_handle.m_UGCQueryHandle);
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "numResultsReturned", data.m_unNumResultsReturned);
            PutUint(logData, "totalMatchingResults", data.m_unTotalMatchingResults);
            PutBool(logData, "cachedData", data.m_bCachedData);
            PutString(logData, "nextCursor", data.m_rgchNextCursor);

            SuperfineSDK.Log(EventNames.UGC_QUERY_COMPLETED, logData);
        }

        public static void LogUGCRequestUGCDetailsResult(ref SteamUGCRequestUGCDetailsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutJSONObject(logData, "details", GetJSONObjectFromSteamUGCDetails(ref data.m_details));
            PutBool(logData, "cachedData", data.m_bCachedData);

            SuperfineSDK.Log(EventNames.UGC_REQUEST_UGC_DETAILS_RESULT, logData);
        }

        public static void LogCreateItemResult(ref CreateItemResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutBool(logData, "userNeedsToAcceptWorkshopLegalAgreement", data.m_bUserNeedsToAcceptWorkshopLegalAgreement);

            SuperfineSDK.Log(EventNames.CREATE_ITEM_RESULT, logData);
        }

        public static void LogSubmitItemUpdateResult(ref SubmitItemUpdateResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutBool(logData, "userNeedsToAcceptWorkshopLegalAgreement", data.m_bUserNeedsToAcceptWorkshopLegalAgreement);

            SuperfineSDK.Log(EventNames.SUBMIT_ITEM_UPDATE_RESULT, logData);
        }

        public static void LogItemInstalled(ref ItemInstalled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID.m_AppId);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.ITEM_INSTALLED, logData);
        }

        public static void LogDownloadItemResult(ref DownloadItemResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID.m_AppId);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.DOWNLOAD_ITEM_RESULT, logData);
        }

        public static void LogUserFavoriteItemsListChanged(ref UserFavoriteItemsListChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "result", (int)data.m_eResult);
            PutBool(logData, "wasAddRequest", data.m_bWasAddRequest);

            SuperfineSDK.Log(EventNames.USER_FAVORITE_ITEMS_LIST_CHANGED, logData);
        }

        public static void LogSetUserItemVoteResult(ref SetUserItemVoteResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "result", (int)data.m_eResult);
            PutBool(logData, "voteUp", data.m_bVoteUp);

            SuperfineSDK.Log(EventNames.SET_USER_ITEM_VOTE_RESULT, logData);
        }

        public static void LogGetUserItemVoteResult(ref GetUserItemVoteResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutInt(logData, "result", (int)data.m_eResult);
            PutBool(logData, "votedUp", data.m_bVotedUp);
            PutBool(logData, "votedDown", data.m_bVotedDown);
            PutBool(logData, "voteSkipped", data.m_bVoteSkipped);

            SuperfineSDK.Log(EventNames.GET_USER_ITEM_VOTE_RESULT, logData);
        }

        public static void LogStartPlaytimeTrackingResultt(ref StartPlaytimeTrackingResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.START_PLAYTIME_TRACKING_RESULT, logData);
        }

        public static void LogStopPlaytimeTrackingResultt(ref StopPlaytimeTrackingResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.STOP_PLAYTIME_TRACKING_RESULT, logData);
        }

        public static void LogAddUGCDependencyResult(ref AddUGCDependencyResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUlong(logData, "childPublishedFileId", data.m_nChildPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.ADD_UGC_DEPENDENCY_RESULT, logData);
        }

        public static void LogRemoveUGCDependencyResult(ref RemoveUGCDependencyResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUlong(logData, "childPublishedFileId", data.m_nChildPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.REMOVE_UGC_DEPENDENCY_RESULT, logData);
        }

        public static void LogAddAppDependencyResult(ref AddAppDependencyResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.ADD_APP_DEPENDENCY_RESULT, logData);
        }

        public static void LogRemoveAppDependencyResult(ref RemoveAppDependencyResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.REMOVE_APP_DEPENDENCY_RESULT, logData);
        }

        public static void LogGetAppDependenciesResult(ref GetAppDependenciesResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);
            PutUint(logData, "totalNumAppDependencies", data.m_nTotalNumAppDependencies);

            AppId_t[] appIds = data.m_rgAppIDs;
            if (appIds != null)
            {
                int numApps = Math.Min(appIds.Length, (int)data.m_nNumAppDependencies);
                if (numApps > 0)
                {
                    SimpleJSON.JSONArray arr = new SimpleJSON.JSONArray();
                    for (int i = 0; i < numApps; ++i)
                    {
                        arr.Add(appIds[i].m_AppId);
                    }

                    PutJSONArray(logData, "appIds", arr);
                }
            }

            SuperfineSDK.Log(EventNames.GET_APP_DEPENDENCIES_RESULT, logData);
        }

        public static void LogDeleteItemResult(ref DeleteItemResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "publishedFileId", data.m_nPublishedFileId.m_PublishedFileId);

            SuperfineSDK.Log(EventNames.DELETE_ITEM_RESULT, logData);
        }

        public static void LogUserSubscribedItemsListChanged(ref UserSubscribedItemsListChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_nAppID.m_AppId);

            SuperfineSDK.Log(EventNames.USER_SUBSCRIBED_ITEMS_LIST_CHANGED, logData);
        }

        public static void LogWorkshopEULAStatus(ref WorkshopEULAStatus_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "appId", data.m_nAppID.m_AppId);
            PutUint(logData, "version", data.m_unVersion);
            PutUint(logData, "action", data.m_rtAction.m_RTime32);
            PutBool(logData, "accepted", data.m_bAccepted);
            PutBool(logData, "needsAction", data.m_bNeedsAction);

            SuperfineSDK.Log(EventNames.WORKSHOP_EULA_STATUS, logData);
        }

        public static void LogSteamServersConnected(ref SteamServersConnected_t data)
        {
            SuperfineSDK.Log(EventNames.STEAM_SERVERS_CONNECTED);
        }

        public static void LogSteamServerConnectFailure(ref SteamServerConnectFailure_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutBool(logData, "stillRetrying", data.m_bStillRetrying);

            SuperfineSDK.Log(EventNames.STEAM_SERVER_CONNECT_FAILURE, logData);
        }

        public static void LogSteamServersDisconnected(ref SteamServersDisconnected_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.STEAM_SERVERS_DISCONNECTED, logData);
        }

        public static void LogClientGameServerDeny(ref ClientGameServerDeny_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_uAppID);
            PutString(logData, "gameServerIp", GetIPv4String(data.m_unGameServerIP));
            PutUint(logData, "gameServerPort", data.m_usGameServerPort);
            PutBool(logData, "secure", data.m_bSecure != 0);
            PutUint(logData, "reason", data.m_uReason);

            SuperfineSDK.Log(EventNames.CLIENT_GAME_SERVER_DENY, logData);
        }

        public static void LogIPCFailure(ref IPCFailure_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutByte(logData, "failureType", data.m_eFailureType);
           
            SuperfineSDK.Log(EventNames.IPC_FAILURE, logData);
        }

        public static void LogLicensesUpdated(ref LicensesUpdated_t data)
        {
            SuperfineSDK.Log(EventNames.LICENSES_UPDATED);
        }

        public static void LogValidateAuthTicketResponse(ref ValidateAuthTicketResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamId", data.m_SteamID.m_SteamID);
            PutInt(logData, "authSessionResponse", (int)data.m_eAuthSessionResponse);
            PutUlong(logData, "ownerSteamId", data.m_OwnerSteamID.m_SteamID);

            SuperfineSDK.Log(EventNames.VALIDATE_AUTH_TICKET_RESPONSE, logData);
        }

        public static void LogMicroTxnAuthorizationResponse(ref MicroTxnAuthorizationResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "appId", data.m_unAppID);
            PutUlong(logData, "orderId", data.m_ulOrderID);
            PutBool(logData, "authorized", data.m_bAuthorized != 0);

            SuperfineSDK.Log(EventNames.MICRO_TXN_AUTHORIZATION_RESPONSE, logData);
        }

        public static void LogEncryptedAppTicketResponse(ref EncryptedAppTicketResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.ENCRYPTED_APP_TICKET_RESPONSE, logData);
        }

        public static void LogGetAuthSessionTicketResponse(ref GetAuthSessionTicketResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "authTicket", data.m_hAuthTicket.m_HAuthTicket);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.GET_AUTH_SESSION_TICKET_RESPONSE, logData);
        }

        public static void LogGameWebCallback(ref GameWebCallback_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "url", data.m_szURL);

            SuperfineSDK.Log(EventNames.GAME_WEB_CALLBACK, logData);
        }

        public static void LogStoreAuthURLResponse(ref StoreAuthURLResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutString(logData, "url", data.m_szURL);

            SuperfineSDK.Log(EventNames.STORE_AUTH_URL_RESPONSE, logData);
        }

        public static void LogMarketEligibilityResponse(ref MarketEligibilityResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "allowed", data.m_bAllowed);
            PutInt(logData, "notAllowedReason", (int)data.m_eNotAllowedReason);
            PutUint(logData, "allowedAtTime", data.m_rtAllowedAtTime.m_RTime32);
            PutInt(logData, "steamGuardRequiredDays", data.m_cdaySteamGuardRequiredDays);
            PutInt(logData, "newDeviceCooldownDays", data.m_cdayNewDeviceCooldown);

            SuperfineSDK.Log(EventNames.MARKET_ELIGIBILITY_RESPONSE, logData);
        }

        public static void LogDurationControl(ref DurationControl_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "appId", data.m_appid.m_AppId);
            PutBool(logData, "applicable", data.m_bApplicable);
            PutInt(logData, "last5hSecs", data.m_csecsLast5h);
            PutInt(logData, "progress", (int)data.m_progress);
            PutInt(logData, "notification", (int)data.m_notification);
            PutInt(logData, "todaySecs", data.m_csecsToday);
            PutInt(logData, "remainingSecs", data.m_csecsRemaining);

            SuperfineSDK.Log(EventNames.DURATION_CONTROL, logData);
        }

        public static void LogGetTicketForWebApiResponse(ref GetTicketForWebApiResponse_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUint(logData, "authTicket", data.m_hAuthTicket.m_HAuthTicket);
            PutInt(logData, "result", (int)data.m_eResult);
            PutByteArray(logData, "ticket", data.m_rgubTicket, data.m_cubTicket);

            SuperfineSDK.Log(EventNames.GET_TICKET_FOR_WEB_API_RESPONSE, logData);
        }

        public static void LogUserStatsReceived(ref UserStatsReceived_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.USER_STATS_RECEIVED, logData);
        }

        public static void LogUserStatsStored(ref UserStatsStored_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.USER_STATS_STORED, logData);
        }

        public static void LogUserAchievementStored(ref UserAchievementStored_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutBool(logData, "groupAchievement", data.m_bGroupAchievement);
            PutString(logData, "achievementName", data.m_rgchAchievementName);
            PutUint(logData, "curProgress", data.m_nCurProgress);
            PutUint(logData, "maxProgress", data.m_nMaxProgress);

            SuperfineSDK.Log(EventNames.USER_ACHIEVEMENT_STORED, logData);
        }

        public static void LogLeaderboardFindResult(ref LeaderboardFindResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamLeaderboard", data.m_hSteamLeaderboard.m_SteamLeaderboard);
            PutBool(logData, "leaderboardFound", data.m_bLeaderboardFound != 0);
            
            SuperfineSDK.Log(EventNames.LEADERBOARD_FIND_RESULT, logData);
        }

        public static void LogLeaderboardScoresDownloaded(ref LeaderboardScoresDownloaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "steamLeaderboard", data.m_hSteamLeaderboard.m_SteamLeaderboard);
            PutUlong(logData, "steamLeaderboardEntries", data.m_hSteamLeaderboardEntries.m_SteamLeaderboardEntries);
            PutInt(logData, "entryCount", data.m_cEntryCount);

            SuperfineSDK.Log(EventNames.LEADERBOARD_SCORES_DOWNLOADED, logData);
        }

        public static void LogLeaderboardScoreUploaded(ref LeaderboardScoreUploaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "success", data.m_bSuccess != 0);
            PutUlong(logData, "steamLeaderboard", data.m_hSteamLeaderboard.m_SteamLeaderboard);
            PutInt(logData, "score", data.m_nScore);
            PutBool(logData, "scoreChanged", data.m_bScoreChanged != 0);
            PutInt(logData, "globalRankNew", data.m_nGlobalRankNew);
            PutInt(logData, "globalRankPrevious", data.m_nGlobalRankPrevious);

            SuperfineSDK.Log(EventNames.LEADERBOARD_SCORE_UPLOADED, logData);
        }

        public static void LogNumberOfCurrentPlayers(ref NumberOfCurrentPlayers_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "success", data.m_bSuccess != 0);
            PutInt(logData, "players", data.m_cPlayers);

            SuperfineSDK.Log(EventNames.NUMBER_OF_CURRENT_PLAYERS, logData);
        }

        public static void LogUserStatsUnloaded(ref UserStatsUnloaded_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "userSteamId", data.m_steamIDUser.m_SteamID);

            SuperfineSDK.Log(EventNames.USER_STATS_UNLOADED, logData);
        }

        public static void LogUserAchievementIconFetched(ref UserAchievementIconFetched_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID.m_GameID);
            PutString(logData, "achievementName", data.m_rgchAchievementName);
            PutBool(logData, "achieved", data.m_bAchieved);
            PutInt(logData, "iconHandle", data.m_nIconHandle);

            SuperfineSDK.Log(EventNames.USER_ACHIEVEMENT_ICON_FETCHED, logData);
        }

        public static void LogGlobalAchievementPercentagesReady(ref GlobalAchievementPercentagesReady_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.GLOBAL_ACHIEVEMENT_PERCENTAGES_READY, logData);
        }

        public static void LogLeaderboardUGCSet(ref LeaderboardUGCSet_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "steamLeaderboard", data.m_hSteamLeaderboard.m_SteamLeaderboard);

            SuperfineSDK.Log(EventNames.LEADERBOARD_UGC_SET, logData);
        }

        /*
        public static void LogPS3TrophiesInstalled(ref PS3TrophiesInstalled_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutInt(logData, "result", (int)data.m_eResult);
            PutUlong(logData, "requiredDiskSpace", data.m_ulRequiredDiskSpace);

            SuperfineSDK.Log(EventNames.PS3_TROPHIES_INSTALLED, logData);
        }
        */

        public static void LogGlobalStatsReceived(ref GlobalStatsReceived_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "gameId", data.m_nGameID);
            PutInt(logData, "result", (int)data.m_eResult);

            SuperfineSDK.Log(EventNames.GLOBAL_STATS_RECEIVED, logData);
        }

        public static void LogIPCountry(ref IPCountry_t data)
        {
            SuperfineSDK.Log(EventNames.IP_COUNTRY);
        }

        public static void LogLowBatteryPower(ref LowBatteryPower_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutByte(logData, "batteryLeftMinutes", data.m_nMinutesBatteryLeft);

            SuperfineSDK.Log(EventNames.LOW_BATTERY_POWER, logData);
        }

        public static void LogSteamAPICallCompleted(ref SteamAPICallCompleted_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutUlong(logData, "asyncCall", data.m_hAsyncCall.m_SteamAPICall);
            PutInt(logData, "callback", data.m_iCallback);
            PutUint(logData, "param", data.m_cubParam);

            SuperfineSDK.Log(EventNames.STEAM_API_CALL_COMPLETED, logData);
        }

        public static void LogSteamShutdown(ref SteamShutdown_t data)
        {
            SuperfineSDK.Log(EventNames.STEAM_SHUTDOWN);
        }

        public static void LogCheckFileSignature(ref CheckFileSignature_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "checkFileSignature", (int)data.m_eCheckFileSignature);

            SuperfineSDK.Log(EventNames.CHECK_FILE_SIGNATURE, logData);
        }

        public static void LogGamepadTextInputDismissed(ref GamepadTextInputDismissed_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutBool(logData, "submitted", data.m_bSubmitted);
            PutUint(logData, "submittedText", data.m_unSubmittedText);
            PutUint(logData, "appId", data.m_unAppID.m_AppId);

            SuperfineSDK.Log(EventNames.GAMEPAD_TEXT_INPUT_DISMISSED, logData);
        }

        public static void LogAppResumingFromSuspend(ref AppResumingFromSuspend_t data)
        {
            SuperfineSDK.Log(EventNames.APP_RESUMING_FROM_SUSPEND);
        }

        public static void LogFloatingGamepadTextInputDismissed(ref FloatingGamepadTextInputDismissed_t data)
        {
            SuperfineSDK.Log(EventNames.FLOATING_GAMEPAD_TEXT_INPUT_DISMISSED);
        }

        public static void LogFilterTextDictionaryChanged(ref FilterTextDictionaryChanged_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "language", data.m_eLanguage);

            SuperfineSDK.Log(EventNames.FILTER_TEXT_DICTIONARY_CHANGED, logData);
        }

        public static void LogGetVideoURLResult(ref GetVideoURLResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "videoAppId", data.m_unVideoAppID.m_AppId);
            PutString(logData, "url", data.m_rgchURL);

            SuperfineSDK.Log(EventNames.GET_VIDEO_URL_RESULT, logData);
        }

        public static void LogGetOPFSettingsResult(ref GetOPFSettingsResult_t data)
        {
            SimpleJSON.JSONObject logData = new SimpleJSON.JSONObject();
            PutInt(logData, "result", (int)data.m_eResult);
            PutUint(logData, "videoAppId", data.m_unVideoAppID.m_AppId);

            SuperfineSDK.Log(EventNames.GET_OPF_SETTINGS_RESULT, logData);
        }
    }
}

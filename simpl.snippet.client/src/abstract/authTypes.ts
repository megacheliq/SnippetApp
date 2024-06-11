export interface IKeycloakResponse {
    access_token: string;
    expires_in: number;
    refresh_expires_in: number;
    refresh_token: string;
    token_type: string;
    not_before_policy: number;
    session_state: string;
    scope: string;
}

export interface IRealmAccess {
    roles: string[];
}

export interface IUser {
    exp: number;
    iat: number;
    jti: string;
    iss: string;
    aud: string[];
    sub: string;
    typ: string;
    azp: string;
    session_state: string;
    acr: string;
    allowed_origins: string[];
    realm_access: IRealmAccess;
    scope: string;
    sid: string;
    name: string;
    email_verified: boolean;
    preferred_username: string;
    given_name: string;
    family_name: string;
    email: string;
    user_id: string;
}
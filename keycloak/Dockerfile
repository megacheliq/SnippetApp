FROM quay.io/keycloak/keycloak:23.0.7

ENV KC_DB=postgres \
    KC_DB_URL=jdbc:postgresql://keycloakdb:5432/keycloak \
    KC_DB_USERNAME=keycloak \
    KC_DB_PASSWORD=password \
    KC_HOSTNAME=localhost \
    KC_HOSTNAME_PORT=8080 \
    KC_HOSTNAME_STRICT=false \
    KC_HOSTNAME_STRICT_HTTPS=false \
    KC_LOG_LEVEL=info \
    KC_METRICS_ENABLED=true \
    KC_HEALTH_ENABLED=true \
    KEYCLOAK_ADMIN=admin \
    KEYCLOAK_ADMIN_PASSWORD=admin

COPY ./keycloak-export/ /opt/keycloak/data/import/

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start-dev", "--import-realm"]
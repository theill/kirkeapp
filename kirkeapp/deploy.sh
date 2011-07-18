#!/bin/bash
API_TOKEN='03437464a3f3e0c263eb711e83c35c69_MTEyOTM'
TEAM_TOKEN='c91f9a7a5176fbe3d640a09e3105f932_MTU5NzQ' # get from https://testflightapp.com/dashboard/team/edit/?next=/dashboard/
BUILDDIR="$(pwd)"
GIT_PATH="$(which git)"
APP_NAME='kirkeapp'
RELEASE_NOTES="$($GIT_PATH show -s --format=%s)"

rm $APP_NAME.ipa
mkdir -p tozip/Payload
cp -r bin/iPhone/Distribution/*.app tozip/Payload
cd tozip/
zip -r ../$APP_NAME.ipa *
cd ..
rm -rf tozip

curl http://testflightapp.com/api/builds.json -F file="@$BUILDDIR/$APP_NAME.ipa" -F api_token="$API_TOKEN" -F team_token="$TEAM_TOKEN" -F notes="$RELEASE_NOTES" -F distribution_lists="Internal" -F notify=True -F replace=True -v --progress-bar

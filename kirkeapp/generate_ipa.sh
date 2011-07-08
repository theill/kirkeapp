rm kirkeapp.ipa
mkdir -p tozip/Payload
cp -r bin/iPhone/Distribution/*.app tozip/Payload
cd tozip/
zip -r ../kirkeapp.ipa *
cd ..
rm -rf tozip

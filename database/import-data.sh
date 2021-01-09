db_password=`printenv SA_PASSWORD`

for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U 'sa' -P $db_password -i all_in_one.sql
    if [ $? -eq 0 ]
    then
        echo "database initialized!"
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done

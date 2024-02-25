import React, { useState, useEffect } from "react";
import { getDatabase, ref, get } from "firebase/database";
import Box from "@mui/material/Box";
import red from "@mui/material/colors/red";

function RealtimeDataDisplay({ databaseRef }) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const database = getDatabase();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const snapshot = await get(databaseRef);
        if (snapshot.exists()) {
          setData(snapshot.val());
        } else {
          setData(null);
        }
        setLoading(false);
      } catch (error) {
        setError(error);
        setLoading(false);
      }
    };

    fetchData();
  }, [databaseRef]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  if (!data) {
    return <div>No data found.</div>;
  }

  return (
    <Box display="flex" flexDirection="column">
      {" "}
      {/* Use Box to create a flex container */}
      <ul>
        {Object.entries(data).map(([timestamp, entry]) => (
          <li key={timestamp}>
            <strong>Timestamp: {timestamp}</strong>
            <ul>
              {Object.entries(entry).map(([id, entryData]) => (
                <li key={id}>
                  <strong>ID: {id}</strong>
                  <p>Data: {entryData}</p>
                </li>
              ))}
            </ul>
          </li>
        ))}
      </ul>
    </Box>
  );
}

export default RealtimeDataDisplay;

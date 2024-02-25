import React, { useState } from "react";
import { ref, getDatabase } from "firebase/database";
import RealtimeDataDisplay from "./display";

function SearchComponent() {
  const [searchValue, setSearchValue] = useState(""); // State to hold the search input value
  const [databaseRef, setDatabaseRef] = useState(null); // State to hold the database reference

  const handleInputChange = (event) => {
    setSearchValue(event.target.value); // Update the searchValue state with the input value
  };

  // Function to set the database reference with the updated searchValue
  const setDatabaseRefWithSearch = () => {
    if (searchValue) {
      const database = getDatabase();
      const newDatabaseRef = ref(database, searchValue); // Create a reference using the searchValue
      setDatabaseRef(newDatabaseRef); // Update the database reference state
    }
  };

  return (
    <div>
      <div style={{ textAlign: "center" }}>
        <input
          type="text"
          placeholder="Search..."
          value={searchValue}
          onChange={handleInputChange}
          style={{ fontSize: "1em", padding: "5px", textAlign: "center" }}
        />
        <button
          onClick={setDatabaseRefWithSearch}
          style={{ fontSize: "1em", padding: "5px", textAlign: "center" }}
        >
          Search
        </button>
      </div>
      {databaseRef && <RealtimeDataDisplay databaseRef={databaseRef} />}
    </div>
  );
}

export default SearchComponent;

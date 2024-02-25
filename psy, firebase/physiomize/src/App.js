import logo from "./logo.svg";
import "./App.css";
import Background from "./Components/Backgroung";
import SearchComponent from "./Components/search";
import firebaseApp from "./Components/firebase";

function App() {
  return (
    <div className="App">
      <Background />
      <SearchComponent />
    </div>
  );
}

export default App;

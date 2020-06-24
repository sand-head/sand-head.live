import { createElement } from '@bikeshaving/crank';
import { renderer } from '@bikeshaving/crank/cjs/dom';
import App from './App';
import { BrowserSignalRProvider } from './components/SignalR';

async function renderApp() {
  return await renderer.render(
    <BrowserSignalRProvider url={process.env.RAZZLE_SIGNALR_HUB!}>
      <App />
    </BrowserSignalRProvider>, document.body);
}

// Initial render.
renderApp();

if (module.hot) {
  module.hot.accept("./App", renderApp);
}
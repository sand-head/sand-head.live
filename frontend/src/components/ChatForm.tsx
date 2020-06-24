import { createElement, Context } from '@bikeshaving/crank';
import { SIGNALR_KEY, SignalRClient } from './SignalR';
import { css } from 'emotion';

export function *ChatForm(this: Context) {
  const client = this.get(SIGNALR_KEY) as SignalRClient;
  const textInputs: { [key: string]: string } = {};

  const formCallback = (event: Event) => {
    if (textInputs['username'] && textInputs['message']) {
      client.send('newMessage', textInputs['username'], textInputs['message']);
    }
    event.preventDefault();
  };
  const textCallback = (id: string) => (event: InputEvent) => {
    textInputs[id] = (event.target! as HTMLInputElement).value;
  };

  while (true) {
    yield (
      <form class={styles} onsubmit={formCallback}>
        <input type="text" id="username" placeholder="Username" oninput={textCallback('username')} />
        <input type="text" id="password" placeholder="Message" oninput={textCallback('message')} />
        <button>Submit</button>
      </form>
    );
  }
}

const styles = css`
  display: grid;
  width: 100%;
  grid-template: auto auto / auto 76px;
  
  #password {
    grid-row: 2 / 2;
  }
  button {
    grid-row: 1 / 3;
  }
`;
import { createElement, Context, Copy } from '@bikeshaving/crank';
import { SignalRClient, SIGNALR_KEY } from './SignalR';

const equals = <T extends {}>(props: T, newProps: T) => {
  for (const key in {...props, ...newProps}) {
    if (props[key] !== newProps[key]) {
      return false;
    }
  }
  return true;
};

interface Message {
  username: string;
  identifier: string;
  message: string;
  timestamp: Date;
}
function *Message(this: Context<Message>, props: Message) {
  yield <div>{props.username}#{props.identifier}: {props.message}</div>;
  for (const newProps of this) {
    if (equals(props, newProps)) {
      yield <Copy />;
    } else {
      yield <div>{newProps.username}#{newProps.identifier}: {newProps.message}</div>;
    }
  }
}
export function* ChatLog(this: Context) {
  const client = this.get(SIGNALR_KEY) as SignalRClient;
  const messages: Message[] = [];
  const signalrCallback = (username: string, identifier: string, message: string) => {
    messages.push({
      username,
      identifier,
      message,
      timestamp: new Date()
    });
    this.refresh();
  };

  try {
    client.on('messageReceived', signalrCallback);
    while (true) {
      yield (
        <div id="messages">
          {messages.map(msg => (
            <Message {...msg} crank-id={msg.identifier + '-' + msg.timestamp.getTime()} />
          ))}
          <ScrollToBottom />
        </div>
      );
    }
  } finally {
    client.off('messageReceived', signalrCallback);
  }
}

function *ScrollToBottom(this: Context) {
  let ref: Element;
  Promise.resolve().then(() => this.refresh());
  ref = yield <span />;

  while (true) {
    new Promise((resolve) => setTimeout(resolve, 10)).then(() => ref.scrollIntoView(true));
    yield <Copy />;
  }
}
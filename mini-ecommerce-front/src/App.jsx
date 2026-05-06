// eslint-disable-next-line no-unused-vars
import React, { Component } from 'react'
export class App extends Component {

  render() {
    return (
      <div className="relative min-h-screen overflow-hidden">
        <video
          className="absolute inset-0 h-full w-full object-cover"
          src="/movies/gojoSukuna.mp4" 
          autoPlay
          muted
          loop
          playsInline
        />

        <div className="relative z-10 flex min-h-screen flex-col items-center justify-center bg-black/40 px-4 text-center text-white">
          <h1 className="text-5xl font-bold sm:text-6xl">Hello World!</h1>
          <p className="mt-4 max-w-2xl text-lg sm:text-2xl">
            Sei que isso é muito básico, mas não sei o que mais eu colocaria para iniciar esse projeto, então sei lá man...
          </p>
        </div>
      </div>
    )
  }
}

export default App

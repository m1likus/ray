using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RayTracing
{
	internal class View :GameWindow
	{
		public int shaderHandle;
		int width, height;
		public View(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			//center
			CenterWindow(new Vector2i(width, height));
			//initialize
			this.width = width;
			this.height = height;
		}
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0, 0, e.Width, e.Height);
			this.width = e.Width;
			this.height = e.Height;
		}
		public void LoadShader()
		{
			shaderHandle = GL.CreateProgram();
			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, LoadShaderSource("shader.vert"));
			GL.CompileShader(vertexShader);

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, LoadShaderSource("shader.frag"));
			GL.CompileShader(fragmentShader);

			GL.AttachShader(shaderHandle, vertexShader);
			GL.AttachShader(shaderHandle, fragmentShader);

			GL.LinkProgram(shaderHandle);

			GL.DetachShader(shaderHandle, vertexShader);
			GL.DetachShader(shaderHandle, fragmentShader);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}
		public static string LoadShaderSource(string filepath)
		{
			string shaderSource = "";
			try
			{
				using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath))
				{
					shaderSource = reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to load shader source file: " + e.Message);
			}
			return shaderSource;
		}
		int vbo_position;


		protected override void OnLoad()
		{
			base.OnLoad();



			Vector3[] vertdata = new Vector3[]{
				new Vector3(-1f,-1f,0f),
				new Vector3( 1f,-1f,0f),
				new Vector3( 1f, 1f,0f),
				new Vector3(-1f, 1f,0f),
			};

			GL.GenBuffers(1, out vbo_position);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
			GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length *
									Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			//GL.Uniform3(uniform_pos, campos);
			//GL.Uniform1(uniform_aspect, AspectRatio);
			GL.UseProgram(shaderHandle);

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

		}

	}




}



